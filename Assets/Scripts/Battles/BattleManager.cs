using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BattleManager : MonoBehaviour
{
    public BattleTypes BattleType;

    public Camera BattleCamera;

    public float DefaultHeadHeight = 5f;
    public Plane FloorPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

    public Quaternion PlayerControllerMoveDirectionQuaternion;

    protected virtual void Awake()
    {
        GameManager.Instance.Cur_BattleManager = this;
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        PlayerSpawnPointManager.Init();

        TeamDict.Clear();
        TeamDict.Add(TeamNumber.Team1, new Team(TeamNumber.Team1));
        TeamDict.Add(TeamNumber.Team2, new Team(TeamNumber.Team2));

        GameManager.Instance.DebugPanel.RefreshLevelName();
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (BoltNetwork.IsServer)
            {
                player.state.UpdateState();
            }
            AddPlayer(player);
        }

        Child_Initialize();
    }

    public abstract void Child_Initialize();

    public bool IsStart = false;

    protected virtual void Update()
    {
        if (BoltNetwork.IsServer)
        {
            if (!IsStart)
            {
                if (Input.GetKeyUp(KeyCode.F4))
                {
                    if (!GameManager.Instance.Cur_BattleManager || GameManager.Instance.Cur_BattleManager.BattleType != BattleTypes.Prepare)
                    {
                        GameManager.Instance.SwitchBattle(BattleTypes.Prepare);
                    }
                }

                if (Input.GetKeyUp(KeyCode.F5))
                {
                    if (!GameManager.Instance.Cur_BattleManager || GameManager.Instance.Cur_BattleManager.BattleType != BattleTypes.FlagRace)
                    {
                        GameManager.Instance.SwitchBattle(BattleTypes.Smash);
                    }
                }

                if (Input.GetKeyUp(KeyCode.F6))
                {
                    if (!GameManager.Instance.Cur_BattleManager || GameManager.Instance.Cur_BattleManager.BattleType != BattleTypes.FlagRace)
                    {
                        GameManager.Instance.SwitchBattle(BattleTypes.FlagRace);
                    }
                }
            }
        }

        GameManager.Instance.DebugPanel.ConfigRows.Refresh();
    }

    #region Players

    public PlayerSpawnPointManager PlayerSpawnPointManager;
    public SortedDictionary<PlayerNumber, Player> PlayerDict = new SortedDictionary<PlayerNumber, Player>();
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

    public virtual void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn(player.PlayerNumber);
    }

    public void ResetAllPlayers()
    {
        if (BoltNetwork.IsServer)
        {
            foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
            {
                ResetPlayer(kv.Value);
                PlayerRingEvent pre = PlayerRingEvent.Create();
                pre.PlayerNumber = (int) kv.Key;
                pre.HasRing = false;
                pre.Exploded = false;
                pre.Send();
            }
        }
    }

    public SortedDictionary<BoltConnection, PlayerInfoData> GetAllPlayerInfoData()
    {
        SortedDictionary<BoltConnection, PlayerInfoData> res = new SortedDictionary<BoltConnection, PlayerInfoData>();
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            if (kv.Value.entity.IsOwner)
            {
                res.Add(BoltNetwork.Server, kv.Value.GetPlayerInfoDate());
            }
            else
            {
                res.Add(kv.Value.entity.Controller, kv.Value.GetPlayerInfoDate());
            }
        }

        return res;
    }

    #endregion
}