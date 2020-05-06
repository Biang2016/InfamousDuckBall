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

        GameManager.Instance.DebugPanel.RefreshScore(true);
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F4/F5/F6 to switch game, F10 to Start/Stop");
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 0, 0);
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

    public override void StartBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            BattleStartEvent.Create().Send();
            StartNewRound_Server();
            ScoreRingManager.Reset();
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.MegaScore = 0;
            }
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
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
        RoundPanel rp = UIManager.Instance.ShowUIForms<RoundPanel>();
        rp.Show(round);

        RoundSmallScorePanel rssp = UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
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
        PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = false;
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
            if (TeamDict[otherTeam].MegaScore < ConfigManager.Smash_TeamTargetMegaScore - 1)
            {
                ScoreChangeEvent _sce_hit = ScoreChangeEvent.Create();
                _sce_hit.TeamNumber = (int) teamNumber;
                _sce_hit.Score = TeamDict[teamNumber].Score - 1;
                TeamDict[teamNumber].Score -= 1;
                _sce_hit.MegaScore = TeamDict[teamNumber].MegaScore;
                _sce_hit.IsNewBattle = false;
                _sce_hit.Send();

                ScoreChangeEvent _sce = ScoreChangeEvent.Create();
                _sce.TeamNumber = (int) otherTeam;
                _sce.Score = TeamDict[otherTeam].Score;
                TeamDict[otherTeam].MegaScore += 1;
                _sce.MegaScore = TeamDict[otherTeam].MegaScore;
                _sce.IsNewBattle = false;
                _sce.Send();
                EndRound_Server(otherTeam, TeamDict[TeamNumber.Team1].MegaScore, TeamDict[TeamNumber.Team2].MegaScore, false);
                return;
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
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.FishBreath, GameManager.Instance.gameObject);
    }

    public override void EndBattle_Server(TeamNumber winnerTeam)
    {
        if (BoltNetwork.IsServer)
        {
            if (Ball)
            {
                BoltNetwork.Destroy(Ball.gameObject);
            }

            BattleEndEvent evnt = BattleEndEvent.Create();
            evnt.BattleType = (int) BattleTypes.Smash;
            evnt.WinnerTeamNumber = (int) winnerTeam;
            evnt.Team1Score = TeamDict[TeamNumber.Team1].MegaScore;
            evnt.Team2Score = TeamDict[TeamNumber.Team2].MegaScore;
            evnt.Send();

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
        UIManager.Instance.CloseUIForm<RoundSmallScorePanel>();
        if (winnerTeam == TeamNumber.None)
        {
        }
        else
        {
            WinPanel wp = UIManager.Instance.ShowUIForms<WinPanel>();
            wp.Initialize(BattleTypes.Smash, winnerTeam);
            wp.Show();
        }

        Ball = null;
        IsStart = false;
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