using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_Smash : BattleManager_BallGame
{
    internal Ball Ball;
    public Transform BallPivot;
    internal Vector3 BallDefaultPos = Vector3.zero;
    public BallValidZone BallValidZone;

    public ScoreRingManager ScoreRingManager => Boat.ScoreRingManager;
    public Boat Boat;

    public override void Child_Initialize()
    {
        base.Child_Initialize();
        if (BoltNetwork.IsServer)
        {
            BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boat.ScoreRingsPivot.position, Boat.ScoreRingsPivot.rotation);
            Boat.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
        }

        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 0, 0);

        UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetPanelPos(true);
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(false);
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players 1/2");
    }

    protected override void Update()
    {
        base.Update();
        if (IsStart && BoltNetwork.IsServer)
        {
            if (Ball)
            {
                Ball.RigidBody.mass = GameManager.Instance.GameState.state.DuckConfig.BallWeight * ConfigManager.Instance.BallWeight;
                Ball.Collider.material.bounciness = GameManager.Instance.GameState.state.DuckConfig.BallBounce * ConfigManager.Instance.BallBounce;
            }
        }
    }

    public override void StartBattleReadyToggle(bool start, int tick)
    {
        if (start)
        {
            UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(false);
            UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Game will start in " + tick + "s");
        }
        else
        {
            if (PlayerDict.Count == 2)
            {
                if (BoltNetwork.IsServer)
                {
                    UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Press F10 to start the game");
                }
                else
                {
                    UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for the game to start");
                }
            }
            else
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players " + PlayerDict.Count + "/2");
            }
        }
    }

    public override void RefreshPlayerNumber(int playerNumber)
    {
        if (BoltNetwork.IsServer)
        {
            if (playerNumber == 2)
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Press F10 to start the game");
            }
            else
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players " + playerNumber + "/2");
            }
        }
    }

    public override void StartBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            if (startBattleCoroutine != null)
            {
                StopCoroutine(startBattleCoroutine);
                startBattleCoroutine = null;
            }

            startBattleCoroutine = StartCoroutine(Co_StartBattle_Server());
        }
    }

    IEnumerator Co_StartBattle_Server()
    {
        for (int i = 0; i < 5; i++)
        {
            BattleReadyStartToggleEvent evnt = BattleReadyStartToggleEvent.Create();
            evnt.Start = true;
            evnt.Tick = 5 - i;
            evnt.Send();
            yield return new WaitForSeconds(1f);
        }

        if (BoltNetwork.IsServer)
        {
            BattleStartEvent.Create().Send();
            ScoreRingManager.Reset();
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.MegaScore = 0;
            }
        }

        StartNewRound_Server();
    }

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(true);
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("");
    }

    public void StartNewRound_Server()
    {
        if (BoltNetwork.IsServer)
        {
            RoundStartEvent evnt = RoundStartEvent.Create();
            int round = 0;
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                round += kv.Value.MegaScore;
            }

            evnt.Round = round + 1;
            evnt.Send();

            if (!Ball)
            {
                BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot.position, BallPivot.rotation);
                Ball = be1.GetComponent<Ball>();
                Ball.state.BallName = "SmashBall";
                Ball.ResetTransform = BallPivot;
                BallDefaultPos = Ball.transform.position;
            }
            else
            {
                Ball.ResetBall();
            }

            ScoreRingManager.Reset();
            ResetAllPlayers();

            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.Score = ConfigManager.Smash_TeamStartScore;
                List<Player> goalies = ClientUtils.GetRandomFromList(kv.Value.TeamPlayers, 1);
                if (goalies.Count > 0)
                {
                    CostumeType ct = ScoreRingManager.GetRingCostumeType(kv.Key);
                    StartCoroutine(Co_PlayerRingRecover(goalies[0], ct));

                    ScoreChangeEvent sce = ScoreChangeEvent.Create();
                    sce.TeamNumber = (int) kv.Key;
                    sce.Score = kv.Value.Score;
                    sce.MegaScore = kv.Value.MegaScore;
                    sce.IsNewBattle = true;
                    sce.Send();
                }
            }
        }
    }

    public void StartNewRound(int round)
    {
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.Round, BattleCamera.gameObject);
        UIManager.Instance.ShowUIForms<RoundPanel>().Show(round);

        RoundSmallScorePanel rssp = UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
        rssp.SetPanelPos(true);
        rssp.RefreshScore_Team1(5);
        rssp.RefreshScore_Team2(5);
    }

    public void EndRound_Server(TeamNumber winnerTeamNumber, int Team1Score, int Team2Score, bool battleEnd)
    {
        StartCoroutine(Co_EndRound_Server(winnerTeamNumber, Team1Score, Team2Score, battleEnd));
    }

    IEnumerator Co_EndRound_Server(TeamNumber winnerTeamNumber, int Team1Score, int Team2Score, bool battleEnd)
    {
        RoundEndEvent evnt = RoundEndEvent.Create();
        evnt.WinTeamNumber = (int) winnerTeamNumber;
        evnt.Team1Score = Team1Score;
        evnt.Team2Score = Team2Score;
        evnt.Send();

        if (battleEnd)
        {
            yield return new WaitForSeconds(3f);
            EndBattle_Server(winnerTeamNumber);
        }
        else
        {
            yield return new WaitForSeconds(3f);
            StartNewRound_Server();
        }
    }

    public void EndRound(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
        {
            PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = false;
        }

        RoundScorePanel rsp = UIManager.Instance.ShowUIForms<RoundScorePanel>();
        rsp.Initialize(team2Score, team1Score);
        rsp.ShowJump();
    }

    public override void BallHit_Server(Ball ball, Player player, TeamNumber teamNumber)
    {
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = false;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.Exploded = true;
        pre.Send();

        Team hitTeam = TeamDict[teamNumber];

        if (hitTeam.Score == 1)
        {
            TeamNumber otherTeam = teamNumber == TeamNumber.Team1 ? TeamNumber.Team2 : TeamNumber.Team1;

            ScoreChangeEvent _sce = ScoreChangeEvent.Create();
            _sce.TeamNumber = (int) otherTeam;
            _sce.Score = TeamDict[otherTeam].Score;
            TeamDict[otherTeam].MegaScore += 1;
            _sce.MegaScore = TeamDict[otherTeam].MegaScore;
            _sce.IsNewBattle = false;
            _sce.Send();

            if (TeamDict[otherTeam].MegaScore < ConfigManager.Smash_TeamTargetMegaScore)
            {
                EndRound_Server(otherTeam, TeamDict[TeamNumber.Team1].MegaScore, TeamDict[TeamNumber.Team2].MegaScore, false);
            }
            else
            {
                EndRound_Server(otherTeam, TeamDict[TeamNumber.Team1].MegaScore, TeamDict[TeamNumber.Team2].MegaScore, true);
            }
        }
        else
        {
            CostumeType ct = ScoreRingManager.GetRingCostumeType(player.TeamNumber);
            Player otherPlayer = null;
            if (TeamDict[teamNumber].TeamPlayers.Count == 1)
            {
                otherPlayer = player;
            }
            else
            {
                List<Player> ps = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1, new List<Player> {player});
                otherPlayer = ps[0];
            }

            StartCoroutine(Co_PlayerRingRecover(otherPlayer, ct));
        }

        hitTeam.Score -= 1;
        ScoreChangeEvent sce = ScoreChangeEvent.Create();
        sce.TeamNumber = (int) teamNumber;
        sce.Score = hitTeam.Score;
        sce.MegaScore = hitTeam.MegaScore;
        sce.IsNewBattle = false;
        sce.Send();

        ball.KickedFly();
        GameManager.Instance.SendSFXEvent(AudioDuck.Instance.FishBreath);
    }

    public override void EndBattle_Server(TeamNumber winnerTeam)
    {
        if (BoltNetwork.IsServer)
        {
            if (startBattleCoroutine != null)
            {
                StopCoroutine(startBattleCoroutine);
                startBattleCoroutine = null;
            }

            {
                BattleReadyStartToggleEvent evnt = BattleReadyStartToggleEvent.Create();
                evnt.Start = false;
                evnt.Tick = 0;
                evnt.Send();
            }

            {
                BattleEndEvent evnt = BattleEndEvent.Create();
                evnt.BattleType = (int) BattleTypes.Smash;
                evnt.WinnerTeamNumber = (int) winnerTeam;
                evnt.Team1Score = TeamDict[TeamNumber.Team1].MegaScore;
                evnt.Team2Score = TeamDict[TeamNumber.Team2].MegaScore;
                evnt.Send();
            }

            if (Ball)
            {
                BoltNetwork.Destroy(Ball.gameObject);
            }

            ResetAllPlayers();
            ScoreRingManager.Reset();
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.Score = ConfigManager.Smash_TeamStartScore;
                kv.Value.MegaScore = 0;
                ScoreChangeEvent sce = ScoreChangeEvent.Create();
                sce.TeamNumber = (int) kv.Key;
                sce.Score = kv.Value.Score;
                sce.MegaScore = kv.Value.MegaScore;
                sce.IsNewBattle = true;
                sce.Send();
            }
        }
    }

    public override void EndBattle(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        base.EndBattle(winnerTeam, team1Score, team2Score);
        if (winnerTeam == TeamNumber.None)
        {
            if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
            {
                PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = true;
            }

            if (IsStart)
            {
                NoticeManager.Instance.ShowInfoPanelTop("GAME ENDS!", 0, 0.5f);
            }

            UIManager.Instance.CloseUIForm<RoundPanel>();
        }
        else
        {
            if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
            {
                PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = true;
            }

            WinPanel wp = UIManager.Instance.ShowUIForms<WinPanel>();
            wp.Initialize(BattleTypes.Smash, winnerTeam);
            wp.Show();
        }

        Ball = null;
        IsStart = false;
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(false);
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
        pre.Exploded = false;
        pre.Send();

        switch (player.TeamNumber)
        {
            case TeamNumber.Team1:
            {
                ScoreRingManager.state.RingNumber_Team1--;
                break;
            }
            case TeamNumber.Team2:
            {
                ScoreRingManager.state.RingNumber_Team2--;
                break;
            }
        }
    }
}