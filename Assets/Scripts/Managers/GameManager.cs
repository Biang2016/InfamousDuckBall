using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private DebugPanel debugPanel;

    internal SortedDictionary<PlayerNumber, Player> PlayerDict = new SortedDictionary<PlayerNumber, Player>();
    internal SortedDictionary<int, Player> RobotDict = new SortedDictionary<int, Player>();
    internal SortedDictionary<TeamNumber, Team> TeamDict = new SortedDictionary<TeamNumber, Team>();

    public const int MaximalPlayerNumber = 4;
    public const int PlayerNumberCount = 4;
    public const int TeamNumberCount = 4;

    internal int LayerMask_RangeOfActivity;
    internal int Layer_RangeOfActivity;
    internal int Layer_PlayerCollider1;
    internal int Layer_PlayerCollider2;
    internal int Layer_PlayerCollider3;
    internal int Layer_PlayerCollider4;
    internal int Layer_BallKicker;
    internal int Layer_Ball;
    internal SortedDictionary<PlayerNumber, int> Layer_PlayerBall = new SortedDictionary<PlayerNumber, int>();

    void Awake()
    {
        Application.targetFrameRate = 60;
        LayerMask_RangeOfActivity = LayerMask.GetMask("RangeOfActivity");
        Layer_RangeOfActivity = LayerMask.NameToLayer("RangeOfActivity");
        Layer_PlayerCollider1 = LayerMask.NameToLayer("PlayerCollider1");
        Layer_PlayerCollider2 = LayerMask.NameToLayer("PlayerCollider2");
        Layer_PlayerCollider3 = LayerMask.NameToLayer("PlayerCollider3");
        Layer_PlayerCollider4 = LayerMask.NameToLayer("PlayerCollider4");
        Layer_BallKicker = LayerMask.NameToLayer("BallKicker");
        Layer_Ball = LayerMask.NameToLayer("Ball");
        Layer_PlayerBall.Add(PlayerNumber.Player1, LayerMask.NameToLayer("Ball1"));
        Layer_PlayerBall.Add(PlayerNumber.Player2, LayerMask.NameToLayer("Ball2"));
        Layer_PlayerBall.Add(PlayerNumber.Player3, LayerMask.NameToLayer("Ball3"));
        Layer_PlayerBall.Add(PlayerNumber.Player4, LayerMask.NameToLayer("Ball4"));
    }

    public bool IsPlayerColliderLayer(int layerIndex)
    {
        return layerIndex == Layer_PlayerCollider1 || layerIndex == Layer_PlayerCollider2 || layerIndex == Layer_PlayerCollider3 || layerIndex == Layer_PlayerCollider4;
    }

    public bool IsBallLayer(int layerIndex)
    {
        return layerIndex == Layer_Ball || Layer_PlayerBall.Values.ToList().Contains(layerIndex);
    }

    private static BattleTypes DefaultBattleType = BattleTypes.PVP4;

    void Start()
    {
        debugPanel = UIManager.Instance.ShowUIForms<DebugPanel>();
        debugPanel.RefreshScore();

        SwitchBattle(DefaultBattleType);
        Input.ResetInputAxes();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10))
        {
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            DefaultBattleType = BattleTypes.PVP4;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F2))
        {
            DefaultBattleType = BattleTypes.PVP4_Bigger;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F3))
        {
            DefaultBattleType = BattleTypes.PVP4_Wall1;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F4))
        {
            DefaultBattleType = BattleTypes.PVP4_Wall2;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F5))
        {
            DefaultBattleType = BattleTypes.PVP4_Wall3;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F6))
        {
            DefaultBattleType = BattleTypes.PVP4_Wall4;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F7))
        {
            DefaultBattleType = BattleTypes.PVP2;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F8))
        {
            DefaultBattleType = BattleTypes.PVP2_Bigger;
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyUp(KeyCode.F9))
        {
            DefaultBattleType = BattleTypes.PVP2_Wall;
            SceneManager.LoadScene("MainScene");
        }
    }

    public bool IsGameStart = true;

    public void Score(TeamNumber kickTeamNumber, TeamNumber hitTeamNumber)
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

        AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        debugPanel.RefreshScore();

        Cur_BattleManager.ResetBall();
    }

    public Vector3 GetBallPosition()
    {
        if (Cur_BattleManager)
        {
            return Cur_BattleManager.Ball.transform.position;
        }

        return Vector3.zero;
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

    public Camera GetCamera()
    {
        if (Cur_BattleManager)
        {
            return Cur_BattleManager.LocalCamera;
        }

        return UIManager.Instance.UICamera;
    }

    public bool IsGameEnd;

    public void SwitchBattle(BattleTypes battleType)
    {
        List<PlayerInfo> pis = new List<PlayerInfo>();
        foreach (KeyValuePair<int, Player> kv in RobotDict)
        {
            if (kv.Value.PlayerInfo.PlayerNumber != PlayerNumber.AI)
            {
                pis.Add(kv.Value.PlayerInfo.Clone());
            }
        }

        foreach (KeyValuePair<int, Player> kv in RobotDict)
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
            foreach (PlayerInfo pi in pis)
            {
                SetUpPlayer(pi);
            }
        }

        TeamDict.Clear();
        TeamDict.Add(TeamNumber.Team1, new Team(TeamNumber.Team1, 0));
        TeamDict.Add(TeamNumber.Team2, new Team(TeamNumber.Team2, 0));
        TeamDict.Add(TeamNumber.Team3, new Team(TeamNumber.Team3, 0));
        TeamDict.Add(TeamNumber.Team4, new Team(TeamNumber.Team4, 0));

        foreach (KeyValuePair<PlayerNumber, Player> kv in PlayerDict)
        {
            TeamDict[kv.Value.PlayerInfo.TeamNumber].TeamPlayers.Add(kv.Value);
        }

        RefreshAllTeamGoal();
        debugPanel.SetScoreShown(true);
        debugPanel.RefreshScore();
        debugPanel.RefreshLevelName();
    }

    public Player SetUpPlayer(PlayerInfo playerInfo)
    {
        if (playerInfo.PlayerNumber != PlayerNumber.AI)
        {
            if (PlayerDict.ContainsKey(playerInfo.PlayerNumber))
            {
                return null;
            }
        }

        Player player = Player.BaseInitialize(playerInfo);
        RobotDict.Add(playerInfo.RobotIndex, player);

        if (playerInfo.PlayerNumber != PlayerNumber.AI)
        {
            PlayerDict.Add(playerInfo.PlayerNumber, player);
        }

        player.PlayerControl.Controllable = IsGameStart;
        Cur_BattleManager.PlayerSpawnPointManager.Spawn(playerInfo);
        Cur_BattleManager.OnSetupPlayer(playerInfo.PlayerNumber);
        return player;
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
                if (p.IsAGoal)
                {
                    currentGoalPlayer.Add(p);
                    p.IsAGoal = false;
                }
            }

            List<Player> validPlayers = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1, currentGoalPlayer);
            if (validPlayers.Count == 0)
            {
                Player goalPlayer = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1)[0];
                goalPlayer.IsAGoal = true;
            }
            else
            {
                Player goalPlayer = validPlayers[0];
                goalPlayer.IsAGoal = true;
            }
        }
    }

    public void ResetPlayer(Player player)
    {
        player.Reset();
        Cur_BattleManager.PlayerSpawnPointManager.Spawn(player.PlayerInfo);
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