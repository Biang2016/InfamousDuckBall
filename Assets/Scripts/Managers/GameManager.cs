using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public float temp = 2f;

    private GameState gameState;

    public GameState GameState
    {
        get
        {
            if (!gameState)
            {
                gameState = FindObjectOfType<GameState>();
            }

            return gameState;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
            AssignLayers();
            Input.ResetInputAxes();
            BoltLauncher.StartClient();
            InvokeRepeating("RepeatUpdateRoomInfo", 0, 2f);
        }
    }

    void RepeatUpdateRoomInfo()
    {
        BoltManager.UpdateCurrentSession();
    }

    public LobbyPanel LobbyPanel;
    public HelpPanel HelpPanel;
    internal LeaveGamePanel LeaveGamePanel;

    public void Start()
    {
        DebugPanel = UIManager.Instance.ShowUIForms<DebugPanel>();
        DebugPanel.CloseUIForm();
        CreateRoomPanel CreateRoomPanel = UIManager.Instance.ShowUIForms<CreateRoomPanel>();
        CreateRoomPanel.CloseUIForm();
        WaitingPanel WaitingPanel = UIManager.Instance.ShowUIForms<WaitingPanel>();
        WaitingPanel.CloseUIForm();
        PasswordPanel PasswordPanel = UIManager.Instance.ShowUIForms<PasswordPanel>();
        PasswordPanel.CloseUIForm();
        GameLogoPanel = UIManager.Instance.ShowUIForms<GameLogoPanel>();
        GameLogoPanel.CloseUIForm();
        RoundSmallScorePanel RoundSmallScorePanel = UIManager.Instance.ShowUIForms<RoundSmallScorePanel>();
        RoundSmallScorePanel.CloseUIForm();
        RoundPanel RoundPanel = UIManager.Instance.ShowUIForms<RoundPanel>();
        RoundPanel.CloseUIForm();
        WinPanel WinPanel = UIManager.Instance.ShowUIForms<WinPanel>();
        WinPanel.CloseUIForm();

        LeaveGamePanel = UIManager.Instance.ShowUIForms<LeaveGamePanel>();
        LeaveGamePanel.CloseUIForm();

        LobbyPanel.Hide();
        HelpPanel.Hide();

        UIManager.Instance.ShowUIForms<CreateNamePanel>();

        AudioDuck.Instance.PlaySound(AudioDuck.Instance.MenuBGM, gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Cur_BattleManager)
            {
                if (!LeaveGamePanel.IsShown)
                {
                    UIManager.Instance.ShowUIForms<LeaveGamePanel>().Initialize();
                }
                else
                {
                    LeaveGamePanel.CloseUIForm();
                }
            }
        }
    }

    public void ReturnToLobby()
    {
        if (Cur_BattleManager)
        {
            StartCoroutine(Co_ReturnToLobby());
        }
    }

    IEnumerator Co_ReturnToLobby()
    {
        if (Cur_BattleManager)
        {
            Cur_BattleManager.IsClosing = true;
            Cur_BattleManager.StopAllCoroutines();
            PlayerObjectRegistry.RemoveAllPlayers();
            BoltNetwork.Shutdown();
            UIManager.Instance.ShowUIForms<WaitingPanel>();
            yield return new WaitForSeconds(3f);
            UIManager.Instance.CloseUIForm<WaitingPanel>();
            BoatMenuManager.Instance.gameObject.SetActive(true);
            BoatMenuManager.Instance.BoatMoveInWithoutGameLogoPanel();
            LobbyPanel.Display();
            UIManager.Instance.CloseUIForm<DebugPanel>();
            SceneManager.LoadScene("BoltMenu");
            BoltLauncher.StartClient();
            BoltManager.UpdateRoomList(BoltNetwork.SessionList, LobbyPanel.CurrentFilter);
            UIManager.Instance.CloseUIForm<RoundSmallScorePanel>();
            UIManager.Instance.CloseUIForm<WinPanel>();
            UIManager.Instance.CloseUIForm<RoundPanel>();

            AudioDuck.Instance.StopAllWOCEvents();
            AudioDuck.Instance.PlaySound(AudioDuck.Instance.MenuBGM, gameObject);
        }
    }

    public BattleManager Cur_BattleManager;

    public BattleManager_BallGame Cur_BallBattleManager
    {
        get
        {
            if (Cur_BattleManager is BattleManager_BallGame)
            {
                return (BattleManager_BallGame) Cur_BattleManager;
            }
            else
            {
                return null;
            }
        }
    }

    public Ball GetBallByHeadPos(Vector3 headPos)
    {
        if (Cur_BallBattleManager)
        {
            if (Cur_BallBattleManager is BattleManager_Smash smash)
            {
                return smash.Ball;
            }

            if (Cur_BallBattleManager is BattleManager_FlagRace flagRace)
            {
                if (flagRace.BallValidZone_Left.Collider.bounds.Contains(headPos))
                {
                    return flagRace.LeftBall;
                }

                if (flagRace.BallValidZone_Right.Collider.bounds.Contains(headPos))
                {
                    return flagRace.RightBall;
                }
            }
        }

        return null;
    }

    internal DebugPanel DebugPanel;
    internal GameLogoPanel GameLogoPanel;

    #region Events

    public void SendSFXEvent(string sfxKey)
    {
        if (BoltNetwork.IsServer)
        {
            SFX_Event evnt = SFX_Event.Create();
            evnt.SoundName = sfxKey;
            evnt.Send();
        }
    }

    #endregion

    #region Utils

    internal int LayerMask_RangeOfActivity;
    internal int Layer_RangeOfActivity;
    internal int Layer_PlayerCollider1;
    internal int Layer_PlayerCollider2;
    internal int Layer_PlayerCollider3;
    internal int Layer_PlayerCollider4;
    internal int Layer_BallKicker;
    internal int Layer_Ball;
    internal SortedDictionary<PlayerNumber, int> Layer_PlayerBall = new SortedDictionary<PlayerNumber, int>();

    private void AssignLayers()
    {
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

    #endregion
}