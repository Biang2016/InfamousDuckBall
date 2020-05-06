﻿using System.Collections;
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

        if (BoltNetwork.IsServer)
        {
            BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boal_Team1.ScoreRingsPivot.position, Boal_Team1.ScoreRingsPivot.rotation);
            Boal_Team1.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
            Boal_Team1.ScoreRingManager.state.RevertColor = true;
            BoltEntity be2 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boal_Team2.ScoreRingsPivot.position, Boal_Team2.ScoreRingsPivot.rotation);
            Boal_Team2.ScoreRingManager = be2.GetComponent<ScoreRingManager>();
            Boal_Team2.ScoreRingManager.state.RevertColor = true;
        }

        ScoreRingManagerDict.Add(TeamNumber.Team1, ScoreRingManager_Team1);
        ScoreRingManagerDict.Add(TeamNumber.Team2, ScoreRingManager_Team2);
        ScoreRingSingleSpawnerDict.Add(TeamNumber.Team1, ScoreRingSingleSpawner_Team1);
        ScoreRingSingleSpawnerDict.Add(TeamNumber.Team2, ScoreRingSingleSpawner_Team2);
        GameManager.Instance.DebugPanel.RefreshScore(false);
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F4/F5/F6 to switch game, F10 to Start/Stop");
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 90, 0);
    }

    protected override void Update()
    {
        base.Update();

        if (IsStart && BoltNetwork.IsServer)
        {
            if (LeftBall)
            {
                LeftBall.RigidBody.mass = GameManager.Instance.GameState.state.DuckConfig.BallWeight * ConfigManager.Instance.BallWeight;
                LeftBall.Collider.material.bounciness = GameManager.Instance.GameState.state.DuckConfig.BallBounce * ConfigManager.Instance.BallBounce;
            }

            if (RightBall)
            {
                RightBall.RigidBody.mass = GameManager.Instance.GameState.state.DuckConfig.BallWeight * ConfigManager.Instance.BallWeight;
                RightBall.Collider.material.bounciness = GameManager.Instance.GameState.state.DuckConfig.BallBounce * ConfigManager.Instance.BallBounce;
            }
        }
    }

    public override void StartBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            ScoreRingManager_Team1.state.RingNumber_Team1 = 0;
            ScoreRingManager_Team1.state.RingNumber_Team2 = 0;
            ScoreRingManager_Team2.state.RingNumber_Team1 = 0;
            ScoreRingManager_Team2.state.RingNumber_Team2 = 0;

            BattleStartEvent.Create().Send();
            if (!LeftBall)
            {
                BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot_Left.position, BallPivot_Left.rotation);
                LeftBall = be1.GetComponent<Ball>();
                LeftBall.state.BallName = "FlagRaceBall_Left";
                LeftBall.ResetTransform = BallPivot_Left;
                BallDefaultPos_Left = LeftBall.transform.position;
            }
            else
            {
                LeftBall.ResetBall();
            }

            if (!RightBall)
            {
                BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot_Right.position, BallPivot_Right.rotation);
                RightBall = be1.GetComponent<Ball>();
                RightBall.state.BallName = "FlagRaceBall_Right";
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
                ScoreChangeEvent sce = ScoreChangeEvent.Create();
                sce.TeamNumber = (int) kv.Key;
                sce.Score = kv.Value.Score;
                sce.IsNewBattle = true;
                sce.Send();
            }

            StartCoroutine(Co_GenerateScoreRingSingle());
        }
    }

    IEnumerator Co_StartBattle()
    {
        PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = false;
        yield return new WaitForSeconds(4f);
        PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = true;
    }

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
        UIManager.Instance.ShowUIForms<RoundPanel>().Show(-1);
        StartCoroutine(Co_StartBattle());
    }

    IEnumerator Co_GenerateScoreRingSingle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                ConfigManager.Instance.RingDropIntervalRandomMin * GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMin,
                ConfigManager.Instance.RingDropIntervalRandomMax * GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMax));
            ScoreRingSingleSpawnerDict[TeamNumber.Team1].Spawn();
            ScoreRingSingleSpawnerDict[TeamNumber.Team2].Spawn();
        }
    }

    public void ResetBall(Ball ball)
    {
        if (BoltNetwork.IsServer)
        {
            if (ball == LeftBall)
            {
            }

            if (ball == RightBall)
            {
            }
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
                    if (scoreRingManager.state.RingNumber_Team1 > 0)
                    {
                        player.Goalie.ParticleRelease();
                        scoreRingManager.state.RingNumber_Team1--;
                        CostumeType ct = ScoreRingManagerDict[player.TeamNumber].GetRingCostumeType(player.TeamNumber);
                        StartCoroutine(Co_PlayerRingRecover(player, ct));
                    }

                    break;
                }
                case TeamNumber.Team2:
                {
                    if (scoreRingManager.state.RingNumber_Team2 > 0)
                    {
                        player.Goalie.ParticleRelease();
                        scoreRingManager.state.RingNumber_Team2--;
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
        if (player.HasRing)
        {
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.HasRing = false;
            pre.PlayerNumber = (int) player.PlayerNumber;
            pre.Exploded = false;
            pre.Send();

            Team scoreTeam = TeamDict[player.TeamNumber];
            ScoreChangeEvent sce = ScoreChangeEvent.Create();
            sce.TeamNumber = (int) player.TeamNumber;
            sce.Score = scoreTeam.Score + 1;
            sce.IsNewBattle = false;
            sce.Send();

            ScoreRingManager srm = ScoreRingManagerDict[player.TeamNumber];
            int myTeamNum = player.TeamNumber == TeamNumber.Team1 ? srm.state.RingNumber_Team1 : srm.state.RingNumber_Team2;
            int otherTeamNum = player.TeamNumber == TeamNumber.Team1 ? srm.state.RingNumber_Team2 : srm.state.RingNumber_Team1;
            if (myTeamNum + otherTeamNum == ScoreRingManager.MaxRingNumber * 2)
            {
                otherTeamNum--;
            }

            myTeamNum++;
            if (player.TeamNumber == TeamNumber.Team1)
            {
                srm.state.RingNumber_Team1 = myTeamNum;
                srm.state.RingNumber_Team2 = otherTeamNum;
            }
            else if (player.TeamNumber == TeamNumber.Team2)
            {
                srm.state.RingNumber_Team1 = otherTeamNum;
                srm.state.RingNumber_Team2 = myTeamNum;
            }

            GameManager.Instance.DebugPanel.RefreshScore(false);
            if (scoreTeam.Score == ConfigManager.FlagRace_TeamTargetScore - 1)
            {
                EndBattle_Server(scoreTeam.TeamNumber);
            }
        }
    }

    public override void BallHit_Server(Ball ball, Player player, TeamNumber teamNumber)
    {
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = false;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.Exploded = true;
        pre.Send();

        ball.KickedFly();
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return null;
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
        pre.Exploded = false;
        pre.Send();
    }

    public void EatDropScoreRingSingle(Player player, ScoreRingSingle scoreRingSingle)
    {
        if (BoltNetwork.IsServer)
        {
            if (!player.HasRing)
            {
                if ((TeamNumber) scoreRingSingle.state.TeamNumber == player.TeamNumber)
                {
                    PlayerRingEvent evnt = PlayerRingEvent.Create();
                    evnt.PlayerNumber = (int) player.PlayerNumber;
                    evnt.CostumeType = scoreRingSingle.state.CostumeType;
                    evnt.HasRing = true;
                    evnt.Exploded = false;
                    evnt.Send();
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
        if (BoltNetwork.IsServer)
        {
            BattleEndEvent evnt = BattleEndEvent.Create();
            evnt.Team1Score = TeamDict[TeamNumber.Team1].Score;
            evnt.Team2Score = TeamDict[TeamNumber.Team2].Score;
            evnt.WinnerTeamNumber = (int) winnerTeam;
            evnt.BattleType = (int) BattleTypes.FlagRace;
            evnt.Send();
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

            ScoreRingSingleSpawner_Team1.Clear();
            ScoreRingSingleSpawner_Team2.Clear();
            ResetAllPlayers();
        }
    }

    public override void EndBattle(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        base.EndBattle(winnerTeam, team1Score, team2Score);
        LeftBall = null;
        RightBall = null;
        IsStart = false;
    }
}