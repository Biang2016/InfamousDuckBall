using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_FlagRace : BattleManager_BallGame
{
    internal Ball LeftBall;
    internal Ball RightBall;
    public Transform BallPivot_Left;
    public Transform BallPivot_Right;
    internal Vector3 BallDefaultPos_Left = Vector3.zero;
    internal Vector3 BallDefaultPos_Right = Vector3.zero;
    public BallValidZone BallValidZone_Left;
    public BallValidZone BallValidZone_Right;

    public Boat Boal_Team1;
    public Boat Boal_Team2;
    public ScoreRingSingleSpawner ScoreRingSingleSpawner_Team1;
    public ScoreRingSingleSpawner ScoreRingSingleSpawner_Team2;
    internal SortedDictionary<TeamNumber, ScoreRingSingleSpawner> ScoreRingSingleSpawnerDict = new SortedDictionary<TeamNumber, ScoreRingSingleSpawner>();

    public ScoreRingManager ScoreRingManager_Team1 => Boal_Team1.ScoreRingManager;
    public ScoreRingManager ScoreRingManager_Team2 => Boal_Team2.ScoreRingManager;

    internal SortedDictionary<TeamNumber, ScoreRingManager> ScoreRingManagerDict = new SortedDictionary<TeamNumber, ScoreRingManager>();

    public override void Child_Initialize()
    {
        base.Child_Initialize();

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (BoltNetwork.IsServer)
            {
                BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boal_Team1.ScoreRingsPivot.position, Boal_Team1.ScoreRingsPivot.rotation);
                Boal_Team1.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
                Boal_Team1.ScoreRingManager.RevertColor = true;
                BoltEntity be2 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boal_Team2.ScoreRingsPivot.position, Boal_Team2.ScoreRingsPivot.rotation);
                Boal_Team2.ScoreRingManager = be2.GetComponent<ScoreRingManager>();
                Boal_Team2.ScoreRingManager.RevertColor = true;
            }
        }
        else
        {
            GameObject be1 = Instantiate(PrefabManager.Instance.GetPrefab("ScoreRings"), Boal_Team1.ScoreRingsPivot.position, Boal_Team1.ScoreRingsPivot.rotation);
            Boal_Team1.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
            Boal_Team1.ScoreRingManager.RevertColor = true;
            GameObject be2 = Instantiate(PrefabManager.Instance.GetPrefab("ScoreRings"), Boal_Team2.ScoreRingsPivot.position, Boal_Team2.ScoreRingsPivot.rotation);
            Boal_Team2.ScoreRingManager = be2.GetComponent<ScoreRingManager>();
            Boal_Team2.ScoreRingManager.RevertColor = true;
        }

        ScoreRingManagerDict.Add(TeamNumber.Team1, ScoreRingManager_Team1);
        ScoreRingManagerDict.Add(TeamNumber.Team2, ScoreRingManager_Team2);
        ScoreRingSingleSpawnerDict.Add(TeamNumber.Team1, ScoreRingSingleSpawner_Team1);
        ScoreRingSingleSpawnerDict.Add(TeamNumber.Team2, ScoreRingSingleSpawner_Team2);
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 90, 0);

        UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetPanelPos(false);
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(false);
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players 1/4");
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (LeftBall)
            {
                LeftBall.RigidBody.mass = ConfigManager.Instance.DuckConfiguration_Multiplier.BallWeightMulti * ConfigManager.Instance.BallWeight;
                LeftBall.Collider.material.bounciness = ConfigManager.Instance.DuckConfiguration_Multiplier.BallBounceMulti * ConfigManager.Instance.BallBounce;
            }

            if (RightBall)
            {
                RightBall.RigidBody.mass = ConfigManager.Instance.DuckConfiguration_Multiplier.BallWeightMulti * ConfigManager.Instance.BallWeight;
                RightBall.Collider.material.bounciness = ConfigManager.Instance.DuckConfiguration_Multiplier.BallBounceMulti * ConfigManager.Instance.BallBounce;
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
            if (PlayerDict.Count == 4)
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
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
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players " + PlayerDict.Count + "/4");
            }
        }
    }

    public override void RefreshPlayerNumber(int playerNumber)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (playerNumber == 4)
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Press F10 to start the game");
            }
            else
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetRoomStatusText("Waiting for other players " + playerNumber + "/4");
            }
        }
    }

    public override void StartBattle_Server()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
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
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                BattleReadyStartToggleEvent evnt = BattleReadyStartToggleEvent.Create();
                evnt.Start = true;
                evnt.Tick = 5 - i;
                evnt.Send();
            }
            else
            {
                Battle_All_Callbacks.OnEvent_BattleReadyStartToggleEvent(true, 5 - i);
            }

            yield return new WaitForSeconds(1f);
        }

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            ScoreRingManager_Team1.RingNumber_Team1 = 0;
            ScoreRingManager_Team1.RingNumber_Team2 = 0;
            ScoreRingManager_Team2.RingNumber_Team1 = 0;
            ScoreRingManager_Team2.RingNumber_Team2 = 0;

            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                BattleStartEvent _evnt = BattleStartEvent.Create();
                _evnt.Send();
            }
            else
            {
                Battle_All_Callbacks.OnEvent_BattleStartEvent();
            }

            if (!LeftBall)
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot_Left.position, BallPivot_Left.rotation);
                    LeftBall = be1.GetComponent<Ball>();
                }
                else
                {
                    GameObject be1 = Instantiate(PrefabManager.Instance.GetPrefab("Ball"), BallPivot_Left.position, BallPivot_Left.rotation);
                    LeftBall = be1.GetComponent<Ball>();
                }

                LeftBall.BallName = "FlagRaceBall_Left";
                LeftBall.ResetTransform = BallPivot_Left;
                BallDefaultPos_Left = LeftBall.transform.position;
            }
            else
            {
                LeftBall.ResetBall();
            }

            if (!RightBall)
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot_Right.position, BallPivot_Right.rotation);
                    RightBall = be1.GetComponent<Ball>();
                }
                else
                {
                    GameObject be1 = Instantiate(PrefabManager.Instance.GetPrefab("Ball"), BallPivot_Right.position, BallPivot_Right.rotation);
                    RightBall = be1.GetComponent<Ball>();
                }

                RightBall.BallName = "FlagRaceBall_Right";
                RightBall.ResetTransform = BallPivot_Right;
                BallDefaultPos_Right = RightBall.transform.position;
            }
            else
            {
                RightBall.ResetBall();
            }

            ResetAllPlayers();

            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.Score = 0;

                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    ScoreChangeEvent evnt = ScoreChangeEvent.Create();
                    evnt.TeamNumber = (int) kv.Key;
                    evnt.Score = kv.Value.Score;
                    evnt.IsNewBattle = true;
                    evnt.Send();
                }
                else
                {
                    Battle_FlagRace_Callbacks.OnEvent_ScoreChangeEvent((int) kv.Key, kv.Value.Score, true, 0);
                }
            }

            StartCoroutine(Co_GenerateScoreRingSingle());
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
        UIManager.Instance.ShowUIForms<RoundPanel>().Show(-1);

        AudioDuck.Instance.PlaySound(AudioDuck.Instance.Round, BattleCamera.gameObject);

        RoundSmallScorePanel rssp = UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
        rssp.SetScoreShown(true);
        rssp.SetPanelPos(false);
        rssp.RefreshScore_Team1(0);
        rssp.RefreshScore_Team2(0);
    }

    IEnumerator Co_GenerateScoreRingSingle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                ConfigManager.Instance.RingDropIntervalRandomMin * ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMinMulti,
                ConfigManager.Instance.RingDropIntervalRandomMax * ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMaxMulti));
            ScoreRingSingleSpawnerDict[TeamNumber.Team1].Spawn();
            ScoreRingSingleSpawnerDict[TeamNumber.Team2].Spawn();
        }
    }

    public void GetFlagRing_Server(Player player)
    {
        if (!player.HasRing)
        {
            TeamNumber otherTeam = player.TeamNumber == TeamNumber.Team1 ? TeamNumber.Team2 : TeamNumber.Team1;
            ScoreRingManager scoreRingManager = ScoreRingManagerDict[otherTeam];

            switch (player.TeamNumber)
            {
                case TeamNumber.Team1:
                {
                    if (scoreRingManager.RingNumber_Team1 > 0)
                    {
                        player.Goalie.ParticleRelease();
                        scoreRingManager.RingNumber_Team1--;
                        CostumeType ct = ScoreRingManagerDict[player.TeamNumber].GetRingCostumeType(player.TeamNumber);
                        StartCoroutine(Co_PlayerRingRecover(player, ct));
                    }

                    break;
                }
                case TeamNumber.Team2:
                {
                    if (scoreRingManager.RingNumber_Team2 > 0)
                    {
                        player.Goalie.ParticleRelease();
                        scoreRingManager.RingNumber_Team2--;
                        CostumeType ct = ScoreRingManagerDict[player.TeamNumber].GetRingCostumeType(player.TeamNumber);
                        StartCoroutine(Co_PlayerRingRecover(player, ct));
                    }

                    break;
                }
            }
        }
    }

    public void FlagScorePointHit_Server(Player player)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.HasRing = false;
            pre.PlayerNumber = (int) player.PlayerNumber;
            pre.Exploded = false;
            pre.Send();
        }
        else
        {
            Battle_All_Callbacks.OnEvent_PlayerRingEvent((int) player.PlayerNumber, false, 0, false);
        }

        Team scoreTeam = TeamDict[player.TeamNumber];

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            ScoreChangeEvent sce = ScoreChangeEvent.Create();
            sce.TeamNumber = (int) player.TeamNumber;
            sce.Score = scoreTeam.Score + 1;
            sce.IsNewBattle = false;
            sce.Send();
        }
        else
        {
            Battle_FlagRace_Callbacks.OnEvent_ScoreChangeEvent((int) player.TeamNumber, scoreTeam.Score + 1, false, 0);
        }

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            SFX_Event sfxEvent = SFX_Event.Create();
            sfxEvent.SoundName = AudioDuck.Instance.Score;
            sfxEvent.Send();
        }
        else
        {
            Battle_All_Callbacks.OnEvent_SFX_Event(AudioDuck.Instance.Score);
        }

        ScoreRingManager srm = ScoreRingManagerDict[player.TeamNumber];
        int myTeamNum = player.TeamNumber == TeamNumber.Team1 ? srm.RingNumber_Team1 : srm.RingNumber_Team2;
        int otherTeamNum = player.TeamNumber == TeamNumber.Team1 ? srm.RingNumber_Team2 : srm.RingNumber_Team1;
        if (myTeamNum + otherTeamNum == ScoreRingManager.MaxRingNumber * 2)
        {
            otherTeamNum--;
        }

        myTeamNum++;
        if (player.TeamNumber == TeamNumber.Team1)
        {
            srm.RingNumber_Team1 = myTeamNum;
            srm.RingNumber_Team2 = otherTeamNum;
        }
        else if (player.TeamNumber == TeamNumber.Team2)
        {
            srm.RingNumber_Team1 = otherTeamNum;
            srm.RingNumber_Team2 = myTeamNum;
        }

        if (scoreTeam.Score == ConfigManager.FlagRace_TeamTargetScore - 1)
        {
            EndBattle_Server(scoreTeam.TeamNumber);
        }
    }

    public override void BallHit_Server(Ball ball, Player player, TeamNumber teamNumber)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.HasRing = false;
            pre.PlayerNumber = (int) player.PlayerNumber;
            pre.Exploded = true;
            pre.Send();
        }
        else
        {
            Battle_All_Callbacks.OnEvent_PlayerRingEvent((int) player.PlayerNumber, false, 0, true);
        }

        ball.KickedFly();
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return null;

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.HasRing = true;
            pre.PlayerNumber = (int) player.PlayerNumber;
            pre.CostumeType = (int) costumeType;
            pre.Exploded = false;
            pre.Send();
        }
        else
        {
            Battle_All_Callbacks.OnEvent_PlayerRingEvent((int) player.PlayerNumber, true, (int) costumeType, false);
        }
    }

    public void EatDropScoreRingSingle(Player player, ScoreRingSingle scoreRingSingle)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (!player.HasRing)
            {
                if ((TeamNumber) scoreRingSingle.TeamNumber == player.TeamNumber)
                {
                    if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                    {
                        PlayerRingEvent evnt = PlayerRingEvent.Create();
                        evnt.PlayerNumber = (int) player.PlayerNumber;
                        evnt.CostumeType = (int) scoreRingSingle.CostumeType;
                        evnt.HasRing = true;
                        evnt.Exploded = false;
                        evnt.Send();
                    }
                    else
                    {
                        Battle_All_Callbacks.OnEvent_PlayerRingEvent((int) player.PlayerNumber, true, (int) scoreRingSingle.CostumeType, false);
                    }

                    scoreRingSingle.Explode(false);
                }
            }
        }
    }

    public override void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn(player.PlayerNumber, player.TeamNumber);
    }

    public override void EndBattle_Server(TeamNumber winnerTeam)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (startBattleCoroutine != null)
            {
                StopCoroutine(startBattleCoroutine);
                startBattleCoroutine = null;
            }

            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    BattleReadyStartToggleEvent evnt = BattleReadyStartToggleEvent.Create();
                    evnt.Start = false;
                    evnt.Tick = 0;
                    evnt.Send();
                }
                else
                {
                    Battle_All_Callbacks.OnEvent_BattleReadyStartToggleEvent(false, 0);
                }
            }

            if (LeftBall)
            {
                LeftBall.StopAllCoroutines();
                BoltNetwork.Destroy(LeftBall.gameObject);
            }

            if (RightBall)
            {
                RightBall.StopAllCoroutines();
                BoltNetwork.Destroy(RightBall.gameObject);
            }

            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    BattleEndEvent evnt = BattleEndEvent.Create();
                    evnt.Team1Score = TeamDict[TeamNumber.Team1].Score;
                    evnt.Team2Score = TeamDict[TeamNumber.Team2].Score;
                    evnt.WinnerTeamNumber = (int) winnerTeam;
                    evnt.BattleType = (int) BattleTypes.FlagRace;
                    evnt.Send();
                }
                else
                {
                    Battle_All_Callbacks.OnEvent_BattleEndEvent((int) BattleTypes.FlagRace, (int) winnerTeam, TeamDict[TeamNumber.Team1].Score, TeamDict[TeamNumber.Team2].Score);
                }
            }

            ScoreRingSingleSpawner_Team1.Clear();
            ScoreRingSingleSpawner_Team2.Clear();
            ResetAllPlayers();
        }
    }

    public override void EndBattle(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        base.EndBattle(winnerTeam, team1Score, team2Score);
        if (winnerTeam == TeamNumber.None)
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                if (PlayerObjectRegistry_Online.MyPlayer && PlayerObjectRegistry_Online.MyPlayer.PlayerController.Controller != null)
                {
                    PlayerObjectRegistry_Online.MyPlayer.PlayerController.Controller.Active = true;
                }
            }
            else
            {
                PlayerObjectRegistry_Local.SetAllPlayerControllerActive(true, true);
            }

            if (IsStart)
            {
                NoticeManager.Instance.ShowInfoPanelTop("GAME ENDS!", 0, 0.5f);
            }

            UIManager.Instance.CloseUIForm<RoundPanel>();
        }
        else
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                if (PlayerObjectRegistry_Online.MyPlayer && PlayerObjectRegistry_Online.MyPlayer.PlayerController.Controller != null)
                {
                    PlayerObjectRegistry_Online.MyPlayer.PlayerController.Controller.Active = true;
                }
            }
            else
            {
                PlayerObjectRegistry_Local.SetAllPlayerControllerActive(true, true);
            }

            WinPanel wp = UIManager.Instance.ShowUIForms<WinPanel>();
            wp.Initialize(BattleTypes.Smash, winnerTeam);
            wp.Show();
        }

        LeftBall = null;
        RightBall = null;
        IsStart = false;
        UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().SetScoreShown(false);
    }
}