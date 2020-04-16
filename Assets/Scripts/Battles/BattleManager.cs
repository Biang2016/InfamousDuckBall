using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleTypes BattleType;

    internal Ball Ball
    {
        get
        {
            if (!ball)
            {
                ball = FindObjectOfType<Ball>();
            }

            return ball;
        }
    }

    private Ball ball;

    internal Vector3 BallDefaultPos = Vector3.zero;
    public Transform BallPivot;
    public float DefaultHeadHeight = 5f;

    public Camera BattleCamera;

    public ScoreRingManager ScoreRingManager;

    public Plane FloorPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

    public void Initialize()
    {
        PlayerSpawnPointManager.Init();

        TeamDict.Clear();
        TeamDict.Add(TeamNumber.Team1, new Team(TeamNumber.Team1, ConfigManager.TeamStartScore));
        TeamDict.Add(TeamNumber.Team2, new Team(TeamNumber.Team2, ConfigManager.TeamStartScore));
        TeamDict.Add(TeamNumber.Team3, new Team(TeamNumber.Team3, ConfigManager.TeamStartScore));
        TeamDict.Add(TeamNumber.Team4, new Team(TeamNumber.Team4, ConfigManager.TeamStartScore));

        GameManager.DebugPanel.RefreshScore();
        GameManager.DebugPanel.SetStartTipShown(true);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10))
        {
            StartGame_Server();
        }

        if (Input.GetKeyUp(KeyCode.F11))
        {
            EndBattle_Server();
        }
    }

    public bool IsStart = false;

    public void StartGame_Server()
    {
        if (BoltNetwork.IsServer)
        {
            BattleStartEvent.Create().Send();
            if (!ball)
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
                kv.Value.Score = ConfigManager.TeamStartScore;
                List<Player> goalies = ClientUtils.GetRandomFromList(kv.Value.TeamPlayers, 1);
                if (goalies.Count > 0)
                {
                    CostumeType ct = GameManager.Cur_BattleManager.ScoreRingManager.GetRingCostumeType(kv.Key);
                    StartCoroutine(Co_PlayerRingRecover(goalies[0], ct));

                    ScoreChangeEvent sce = ScoreChangeEvent.Create();
                    sce.TeamNumber = (int) kv.Key;
                    sce.Score = kv.Value.Score - 1;
                    sce.Send();
                }
            }
        }
    }

    public void StartGame()
    {
        IsStart = true;
        ScoreRingManager.Reset();
        GameManager.DebugPanel.SetStartTipShown(false);
    }

    public void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn(player.PlayerNumber);
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

    public void Score_Server(Player hitPlayer, TeamNumber hitTeamNumber)
    {
        if (BattleType.ToString().Contains("PVP4"))
        {
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.HasRing = false;
            pre.PlayerNumber = (int) hitPlayer.PlayerNumber;
            pre.Send();

            Team hitTeam = TeamDict[hitTeamNumber];

            if (hitTeam.Score == 0)
            {
                EndBattle_Server();
            }
            else
            {
                CostumeType ct = GameManager.Cur_BattleManager.ScoreRingManager.GetRingCostumeType(hitPlayer.TeamNumber);
                Player otherPlayer = null;
                if (TeamDict[hitTeamNumber].TeamPlayers.Count == 1)
                {
                    otherPlayer = hitPlayer;
                }
                else
                {
                    List<Player> ps = ClientUtils.GetRandomFromList(TeamDict[hitTeamNumber].TeamPlayers, 1, new List<Player> {hitPlayer});
                    otherPlayer = ps[0];
                }

                StartCoroutine(Co_PlayerRingRecover(otherPlayer, ct));
            }

            ScoreChangeEvent sce = ScoreChangeEvent.Create();
            sce.TeamNumber = (int) hitTeamNumber;
            sce.Score = hitTeam.Score - 1;
            sce.Send();
        }

        ResetBall();
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
        pre.Send();
    }

    public void EndBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            BattleEndEvent.Create().Send();
            if (ball)
            {
                BoltNetwork.Destroy(ball.gameObject);
            }

            ResetAllPlayers();
        }
    }

    public void EndBattle()
    {
        ball = null;
        IsStart = false;
        GameManager.DebugPanel.SetStartTipShown(true);
    }

    #region Players

    public PlayerSpawnPointManager PlayerSpawnPointManager;
    private SortedDictionary<PlayerNumber, Player> PlayerDict = new SortedDictionary<PlayerNumber, Player>();
    internal SortedDictionary<TeamNumber, Team> TeamDict = new SortedDictionary<TeamNumber, Team>();

    public void AddPlayer(Player player)
    {
        if (!PlayerDict.ContainsKey(player.PlayerNumber))
        {
            PlayerDict.Add(player.PlayerNumber, player);
        }
        else
        {
            PlayerDict[player.PlayerNumber] = player;
        }

        if (!TeamDict[player.TeamNumber].TeamPlayers.Contains(player))
        {
            TeamDict[player.TeamNumber].TeamPlayers.Add(player);
            GameManager.DebugPanel.RefreshScore();
        }
    }

    public Player GetPlayer(PlayerNumber playerNumber)
    {
        PlayerDict.TryGetValue(playerNumber, out Player player);
        return player;
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

    public void ResetAllPlayers()
    {
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            ResetPlayer(kv.Value);
            PlayerRingEvent pre = PlayerRingEvent.Create();
            pre.PlayerNumber = (int) kv.Key;
            pre.HasRing = false;
            pre.Send();
        }
    }

    #endregion
}