using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleTypes BattleType;

    internal Vector3 BallDefaultPos = Vector3.zero;
    public Transform BallPivot;
    internal Ball Ball;

    public Camera BattleCamera;

    public PlayerSpawnPointManager PlayerSpawnPointManager;

    internal SortedDictionary<PlayerNumber, Player> PlayerDict = new SortedDictionary<PlayerNumber, Player>();
    internal SortedDictionary<TeamNumber, Team> TeamDict = new SortedDictionary<TeamNumber, Team>();

    private DebugPanel debugPanel;

    public Plane FloorPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

    public void Initialize()
    {
        debugPanel = UIManager.Instance.ShowUIForms<DebugPanel>();
        debugPanel.RefreshScore();

        PlayerSpawnPointManager.Init();

        TeamDict.Clear();
        TeamDict.Add(TeamNumber.Team1, new Team(TeamNumber.Team1, 0));
        TeamDict.Add(TeamNumber.Team2, new Team(TeamNumber.Team2, 0));
        TeamDict.Add(TeamNumber.Team3, new Team(TeamNumber.Team3, 0));
        TeamDict.Add(TeamNumber.Team4, new Team(TeamNumber.Team4, 0));

        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            TeamDict[kv.Value.TeamNumber].TeamPlayers.Add(kv.Value);
        }

        RefreshAllTeamGoal();
        StartGame();
    }

    public bool IsStart = false;

    public void StartGame()
    {
        if (BoltNetwork.IsServer)
        {
            IsStart = true;
            BattleStartEvent.Create().Send();
            GameObject ball_go = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot.position, BallPivot.rotation);
            Ball = ball_go.GetComponent<Ball>();
            Ball.Collider.enabled = true;
            BallDefaultPos = Ball.transform.position;
        }
    }

    public void RefreshAllTeamGoal()
    {
        foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
        {
            RefreshTeamGoal(kv.Key);
        }
    }

    public void RefreshTeamGoal(TeamNumber teamNumber)
    {
        if (TeamDict[teamNumber].TeamPlayers.Count != 0)
        {
            List<Player> currentGoalPlayer = new List<Player>();

            foreach (Player p in TeamDict[teamNumber].TeamPlayers)
            {
                if (p.Goalie.IsAGoalie)
                {
                    currentGoalPlayer.Add(p);
                    p.Goalie.IsAGoalie = false;
                }
            }

            List<Player> validPlayers = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1, currentGoalPlayer);
            if (validPlayers.Count == 0)
            {
                Player goalPlayer = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1)[0];
                goalPlayer.Goalie.IsAGoalie = true;
            }
            else
            {
                Player goalPlayer = validPlayers[0];
                goalPlayer.Goalie.IsAGoalie = true;
            }
        }
    }

    public void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn(player.PlayerNumber);
    }

    public void EndBattle()
    {
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            DestroyImmediate(kv.Value.gameObject);
        }

        PlayerDict.Clear();

        debugPanel.SetScoreShown(true);
        debugPanel.RefreshScore();
        debugPanel.RefreshLevelName();

        DestroyImmediate(gameObject);
    }

    public void ResetBall()
    {
        if (BoltNetwork.IsServer)
        {
            StartCoroutine(Co_ResetBall(1f));
        }
    }

    IEnumerator Co_ResetBall(float suspendingTime)
    {
        Ball.RigidBody.DOPause();
        Ball.transform.position = BallDefaultPos;
        Ball.Reset();
        Ball.RigidBody.useGravity = false;
        yield return new WaitForSeconds(suspendingTime);
        Ball.RigidBody.useGravity = true;
    }

    public void Score(TeamNumber kickTeamNumber, TeamNumber hitTeamNumber)
    {
        if (BattleType.ToString().Contains("PVP4"))
        {
            if (kickTeamNumber == hitTeamNumber)
            {
                TeamDict[kickTeamNumber].Score--;
                RefreshTeamGoal(kickTeamNumber);
            }
            else
            {
                TeamDict[kickTeamNumber].Score++;
                RefreshTeamGoal(hitTeamNumber);
            }
        }

        if (BattleType.ToString().Contains("PVP2"))
        {
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                if (kv.Key != hitTeamNumber)
                {
                    kv.Value.Score++;
                    RefreshTeamGoal(hitTeamNumber);
                }
            }
        }

        AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        debugPanel.RefreshScore();
        ResetBall();
    }

    public List<Vector3> GetAllPlayerPositions()
    {
        List<Vector3> res = new List<Vector3>();
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            res.Add(kv.Value.transform.position);
        }

        return res;
    }
}