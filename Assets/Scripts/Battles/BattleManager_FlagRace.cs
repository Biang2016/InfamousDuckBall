using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleManager_FlagRace : BattleManager_BallGame
{
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
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to START");
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 90, 0);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void StartBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            ScoreRingManager_Team1.state.RingNumber_Team1 = 0;
            ScoreRingManager_Team1.state.RingNumber_Team2 = 5;
            ScoreRingManager_Team2.state.RingNumber_Team1 = 5;
            ScoreRingManager_Team2.state.RingNumber_Team2 = 0;

            BattleStartEvent.Create().Send();
            if (!Ball)
            {
                BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot.position, BallPivot.rotation);
                BallDefaultPos = Ball.transform.position;
            }
            else
            {
                ResetBall();
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

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to Start/Stop, F4/F5/F6 to switch game");
    }

    IEnumerator Co_GenerateScoreRingSingle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(
                ConfigManager.Instance.RingDropIntervalRandomMin * GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMin,
                ConfigManager.Instance.RingDropIntervalRandomMax * GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMax));
            ScoreRingSingleSpawnerDict[(TeamNumber) Random.Range(0, 2)].Spawn();
        }
    }

    public override void ResetBall()
    {
        if (BoltNetwork.IsServer)
        {
            StartCoroutine(Co_ResetBall(1f));
        }
    }

    IEnumerator Co_ResetBall(float suspendingTime)
    {
        if (Ball)
        {
            Ball.RigidBody.DOPause();
            Ball.transform.position = BallDefaultPos;
            Ball.Reset();
            Ball.RigidBody.useGravity = false;
            yield return new WaitForSeconds(suspendingTime);
            if (Ball) Ball.RigidBody.useGravity = true;
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
                GameManager.Instance.DebugPanel.Wins(scoreTeam.TeamNumber);
                EndBattle_Server();
            }
        }
    }

    public override void BallHit_Server(Player player, TeamNumber teamNumber)
    {
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = false;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.Send();

        ResetBall();
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return new WaitForSeconds(0.2f);
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
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
                    evnt.Send();
                    scoreRingSingle.Explode();
                }
            }
        }
    }

    public override void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn((PlayerNumber) player.TeamNumber);
    }

    public override void EndBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            BattleEndEvent.Create().Send();
            if (Ball)
            {
                BoltNetwork.Destroy(Ball.gameObject);
            }

            ResetAllPlayers();
        }
    }

    public override void EndBattle()
    {
        base.EndBattle();
        ball = null;
        IsStart = false;
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to Start/Stop, F4/F5/F6 to switch game");
    }
}