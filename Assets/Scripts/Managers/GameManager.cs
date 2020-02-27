using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private DebugPanel debugPanel;

    internal Dictionary<PlayerNumber, Player> PlayerDict = new Dictionary<PlayerNumber, Player>();

    public float PlayerRadius;

    internal int LayerMask_RangeOfActivity;
    internal int Layer_RangeOfActivity;
    internal int Layer_PlayerCollider;

    void Awake()
    {
        Application.targetFrameRate = 60;
        LayerMask_RangeOfActivity = LayerMask.GetMask("RangeOfActivity");
        Layer_RangeOfActivity = LayerMask.NameToLayer("RangeOfActivity");
        Layer_PlayerCollider = LayerMask.NameToLayer("PlayerCollider");
    }

    void Start()
    {
        debugPanel = UIManager.Instance.ShowUIForms<DebugPanel>();
        debugPanel.RefreshScore();
        UIManager.Instance.ShowUIForms<CameraDividePanel>();

        SwitchBattle(BattleTypes.PVP);
        SetUpPlayer(PlayerNumber.Player1);
        SetUpPlayer(PlayerNumber.Player2);
        Input.ResetInputAxes();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public bool IsGameStart = true;

    public void Score(PlayerNumber scorePlayer, PlayerNumber hitPlayer)
    {
        PlayerDict[scorePlayer].Score++;
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            if (kv.Key == scorePlayer)
            {
                kv.Value.Score++;
            }

            if (kv.Key == hitPlayer)
            {
                kv.Value.ParticleSystem.Play();
            }
        }

        debugPanel.RefreshScore();
        Cur_BattleManager.ResetBall();
    }

    public List<Vector3> GetAllPlayerPositions()
    {
        List<Vector3> playerPositions = new List<Vector3>();
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            playerPositions.Add(kv.Value.GetPlayerPosition);
        }

        return playerPositions;
    }

    public BattleManager Cur_BattleManager;
    public bool IsGameEnd;

    public void SwitchBattle(BattleTypes battleType)
    {
        List<PlayerNumber> pns = PlayerDict.Keys.ToList();
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            kv.Value.Reset();
        }

        IsGameStart = false;

        bool clearPlayer = Cur_BattleManager && Cur_BattleManager.ClearPlayer;
        if (Cur_BattleManager)
        {
            DestroyImmediate(Cur_BattleManager.gameObject);
            foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
            {
                DestroyImmediate(kv.Value.gameObject);
            }

            PlayerDict.Clear();
        }

        if (IsGameEnd)
        {
            Cur_BattleManager = null;
            EndGame();
            return;
        }

        GameObject battlePrefab = PrefabManager.Instance.GetPrefab("Battle_" + battleType);
        GameObject battle_go = Instantiate(battlePrefab);
        BattleManager battleManager = battle_go.GetComponent<BattleManager>();

        Cur_BattleManager = battleManager;
        Cur_BattleManager.Initialize();
        if (!clearPlayer)
        {
            foreach (PlayerNumber pn in pns)
            {
                SetUpPlayer(pn);
            }
        }
    }

    public Player SetUpPlayer(PlayerNumber playerNumber)
    {
        if (PlayerDict.ContainsKey(playerNumber))
        {
            return null;
        }

        GameObject playerPrefab = PrefabManager.Instance.GetPrefab("Player");
        GameObject playerGO = Instantiate(playerPrefab);
        Player player = playerGO.GetComponent<Player>();
        player.Initialize(playerNumber);
        PlayerDict.Add(playerNumber, player);
        player.PlayerControl.Controllable = IsGameStart;

        Cur_BattleManager.PlayerSpawnPointManager.AddRevivePlayer(playerNumber, 0);
        Cur_BattleManager.OnSetupPlayer(playerNumber);
        return player;
    }

    public void EndGame()
    {
        IsGameEnd = true;
        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            kv.Value.PlayerControl.Controllable = false;
        }
    }
}

public enum JoystickAxis
{
    None = 0,
    L_H = 1,
    L_V = 2,
    R_H = 3,
    R_V = 4,
    Trigger = 5,
    DH = 6,
    DV = 7,
}

public enum JoystickButton
{
    None = 0,
    LB = 1,
    RB = 2,
}