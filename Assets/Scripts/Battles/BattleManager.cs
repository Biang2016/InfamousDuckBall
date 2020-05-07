using System.Collections.Generic;
using UnityEngine;

public abstract class BattleManager : MonoBehaviour
{
    public BattleTypes BattleType;

    public Camera BattleCamera;

    public float DefaultHeadHeight = 5f;
    public Plane FloorPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

    internal Quaternion PlayerControllerMoveDirectionQuaternion;

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
        UIManager.Instance.CloseUIForm<PasswordPanel>();
        UIManager.Instance.CloseUIForm<CreateRoomPanel>();
        UIManager.Instance.CloseUIForm<WaitingPanel>();
        UIManager.Instance.CloseUIForm<MakerPanel>();
        GameManager.Instance.LobbyPanel.gameObject.SetActive(false);
        BoatMenuManager.Instance.gameObject.SetActive(false);

        AudioDuck.Instance.StopAllWOCEvents();
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.Sea, gameObject);

        TeamDict.Clear();
        TeamDict.Add(TeamNumber.Team1, new Team(TeamNumber.Team1));
        TeamDict.Add(TeamNumber.Team2, new Team(TeamNumber.Team2));

        GameManager.Instance.DebugPanel.ConfigRows.Initialize();
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
    public bool IsClosing = false;

    protected virtual void Update()
    {
        GameManager.Instance.DebugPanel.ConfigRows.Refresh();

#if DEBUG
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (UIManager.Instance.GetBaseUIForm<DebugPanel>().IsShown)
            {
                UIManager.Instance.CloseUIForm<DebugPanel>();
            }
            else
            {
                UIManager.Instance.ShowUIForms<DebugPanel>();
            }
        }
#endif
    }

    #region Players

    public TeamSpawnPointManager PlayerSpawnPointManager;
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
            if (kv.Value)
            {
                res.Add(kv.Value.transform.position);
            }
        }

        return res;
    }

    public virtual void ResetPlayer(Player player)
    {
        player.Reset();
        PlayerSpawnPointManager.Spawn(player.PlayerNumber, player.TeamNumber);
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

    #endregion
}