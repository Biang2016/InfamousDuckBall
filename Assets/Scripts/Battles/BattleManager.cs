using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public BattleTypes BattleType;
    public BattleTypes NextBattleType;

    public bool CanPause = true;

    protected bool Initialized = false;

    public GoalBall Ball;
    private Vector3 BallDefaultPos = Vector3.zero;

    public PlayerSpawnPointManager PlayerSpawnPointManager;
    [SerializeField] private BoxCollider RangeOfActivity;

    public float X_Min => RangeOfActivity.bounds.center.x - RangeOfActivity.bounds.extents.x;
    public float X_Max => RangeOfActivity.bounds.center.x + RangeOfActivity.bounds.extents.x;
    public float Z_Min => RangeOfActivity.bounds.center.z - RangeOfActivity.bounds.extents.z;
    public float Z_Max => RangeOfActivity.bounds.center.z + RangeOfActivity.bounds.extents.z;

    public enum CameraTypes
    {
        GlobalMultiplayerFollowCamera,
        LocalCamera,
        PlayerPrivateFollowCamera,
    }

    public CameraTypes CameraType = CameraTypes.GlobalMultiplayerFollowCamera;
    public Camera LocalCamera;

    public bool ClearPlayer = false;

    [SerializeField] private bool UseCountDown = false;
    [SerializeField] private float BattleEndTimeCountDown = 30f;
    [SerializeField] private Text EndGameTickDownText;

    [SerializeField] private Text CountDownTimerText;
    [SerializeField] private Text CountDownTimerMilisecondText;

    protected float BattleTimeTick;
    public bool EndGameByDuration = false;
    public float BattleDuration = 180f;
    protected bool IsBattleTimeOut = false;

    protected bool isBattleEnd = false; // True when battle is ending. To avoid double check

    public bool IsBattleEnd => isBattleEnd;

    public void Initialize()
    {
        Initialized = false;
        BallDefaultPos = Ball.transform.position;
        isBattleEnd = false;
        IsBattleTimeOut = false;
        BattleTimeTick = 0;
        PlayerSpawnPointManager.Init();
        Child_Initialize();
        GameManager.Instance.IsGameStart = true;
        Child_LateInitialize();
        Initialized = true;
    }

    public void ResetBall()
    {
        Ball.transform.position = BallDefaultPos;
        Ball.Reset();
    }

    protected virtual void Child_Initialize()
    {
    }

    protected virtual void Child_LateInitialize()
    {
    }

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    private bool isCountDown30SecondsStarted = false;

    protected virtual void Update()
    {
        BattleTimeTick += Time.deltaTime;
        IsBattleTimeOut = BattleTimeTick > BattleDuration;

        if (UseCountDown)
        {
            if (BattleTimeTick > BattleDuration - BattleEndTimeCountDown && BattleTimeTick < BattleDuration)
            {
                float remainTime = BattleDuration - BattleTimeTick;

                if (remainTime <= 30f && !isCountDown30SecondsStarted)
                {
                    isCountDown30SecondsStarted = true;
                }

                if (EndGameTickDownText)
                {
                    EndGameTickDownText.enabled = true;

                    int tickDownInt = Mathf.CeilToInt(BattleDuration - BattleTimeTick);
                    EndGameTickDownText.text = tickDownInt.ToString();
                }

                if (CountDownTimerText)
                {
                    CountDownTimerText.text = ClientUtils.TimeToString(BattleDuration - BattleTimeTick);
                }

                if (CountDownTimerMilisecondText)
                {
                    CountDownTimerMilisecondText.text = ClientUtils.TimeToString_Milisecond(BattleDuration - BattleTimeTick);
                }
            }
            else
            {
                if (EndGameTickDownText)
                {
                    EndGameTickDownText.enabled = false;
                }
            }
        }
    }

    public virtual void OnPlayerDie(Player player)
    {
    }

    /// <summary>
    /// Executes every frame
    /// </summary>
    public IEnumerator CheckGoNextBattle()
    {
        if (IsBattleEnd) yield return null;
        if (IsBattleComplete())
        {
            isBattleEnd = true;
            GameManager.Instance.IsGameStart = false;
            yield return OnBattleComplete();
            GameManager.Instance.SwitchBattle(NextBattleType);
        }

        yield return null;
    }

    protected virtual bool IsBattleComplete()
    {
        if (EndGameByDuration)
        {
            if (IsBattleTimeOut) return true;
        }

        return false;
    }

    protected virtual IEnumerator OnBattleComplete()
    {
        yield return null;
    }

    public void Debug_BeginTickDown()
    {
        BattleTimeTick = BattleDuration - 5f;
    }

    public virtual void OnSetupPlayer(PlayerNumber playerNumber)
    {
    }
}