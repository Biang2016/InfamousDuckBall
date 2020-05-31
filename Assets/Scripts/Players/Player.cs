using System;
using System.Collections;
using Bolt;
using Boo.Lang;
using UnityEngine;

public class Player : EntityBehaviour<IPlayerState>
{
    #region Local

    private string playerName_Local;
    private PlayerNumber playerNumber_Local;
    private TeamNumber teamNumber_Local;
    private CostumeType costumeType_Local;

    #endregion

    public string PlayerName
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.PlayerInfo.PlayerName;
            }
            else
            {
                return playerName_Local;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.PlayerInfo.PlayerName = value;
            }
            else
            {
                playerName_Local = value;
            }
        }
    }

    public PlayerNumber PlayerNumber
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return (PlayerNumber) state.PlayerInfo.PlayerNumber;
            }
            else
            {
                return playerNumber_Local;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.PlayerInfo.PlayerNumber = (int) value;
            }
            else
            {
                playerNumber_Local = value;
            }
        }
    }

    public TeamNumber TeamNumber
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return (TeamNumber) state.PlayerInfo.TeamNumber;
            }
            else
            {
                return teamNumber_Local;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.PlayerInfo.TeamNumber = (int) value;
            }
            else
            {
                teamNumber_Local = value;
            }
        }
    }

    public CostumeType CostumeType
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return (CostumeType) state.PlayerInfo.CostumeType;
            }
            else
            {
                return costumeType_Local;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.PlayerInfo.CostumeType = (int) value;
            }
            else
            {
                costumeType_Local = value;
            }
        }
    }

    private Vector3 headTargetPosition;

    public Vector3 HeadTargetPosition
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.HeadTargetPosition;
            }
            else
            {
                return HeadTargetPosition;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.HeadTargetPosition = value;
            }
            else
            {
                HeadTargetPosition = value;
            }
        }
    }

    private Vector3 headLookAtPosition;

    public Vector3 HeadLookAtPosition
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.HeadLookAtPosition;
            }
            else
            {
                return headLookAtPosition;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.HeadLookAtPosition = value;
            }
            else
            {
                headLookAtPosition = value;
            }
        }
    }

    private Vector3 feetPosition;

    public Vector3 FeetPosition
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.FeetPosition;
            }
            else
            {
                return feetPosition;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.FeetPosition = value;
            }
            else
            {
                feetPosition = value;
            }
        }
    }

    public bool HasRing;

    public PlayerController PlayerController;
    public PlayerCostume PlayerCostume;

    public Controller Controller
    {
        get => PlayerController.Controller;
        set => PlayerController.Controller = value;
    }

    public Duck Duck;
    public Goalie Goalie;
    public DuckConfig DuckConfig;
    public PlayerCollider PlayerCollider;

    void Awake()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local)
        {
            PlayerController.Attached();
            Duck.Attached();
        }
    }

    public override void Attached()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            PlayerController.Attached();
            Duck.Attached();
            state.OnUpdateState += OnStateChanged;
        }
    }

    public override void Detached()
    {
        base.Detached();
        Duck.Detached();
    }

    void OnDestroy()
    {
        if (GameManager.Instance && GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local)
        {
            Duck.Detached();
        }
    }

    public override void ControlGained()
    {
        base.ControlGained();
        Duck.Neck.NeckTargetPos = Duck.Body.transform.position + Duck.Body.transform.forward * 5f + Duck.Body.transform.up * 3f;
        Duck.Body.Cur_HeadTargetPosition = Duck.Neck.NeckTargetPos;
        Duck.Neck.NeckDeform();
        Duck.Body.MoveNeckTo(Duck.Neck.NeckTargetPos);
    }

    public void OnStateChanged()
    {
        if (!entity.IsOwner && !IsInitialized)
        {
            Initialize();
        }
    }

    private bool IsInitialized = false;

    public void Initialize_Server(string playerName, PlayerNumber playerNumber, TeamNumber teamNumber, CostumeType costumeType)
    {
        PlayerName = playerName;
        PlayerNumber = playerNumber;
        TeamNumber = teamNumber;
        CostumeType = CostumeType;

        if (IsInitialized) return;
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (entity.IsOwner)
            {
                Initialize();
            }
        }
        else
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        IsInitialized = true;
        GameManager.Instance.Cur_BattleManager.AddPlayer(this);
        PlayerCostume.Initialize(PlayerNumber, TeamNumber, CostumeType);
        PlayerCollider.Initialize(this);
        Duck.Initialize();
        Goalie.IsGoalie = false;
    }

    public void Reviving()
    {
    }

    public override void SimulateOwner()
    {
        state.UpdateState();
    }

    public override void SimulateController()
    {
        PlayerController.SimulateController();
        Duck.Body.SimulateController();
    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerController.ExecuteCommand(command, resetState);
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (entity.HasControl && Controller == null)
            {
                if (MultiControllerManager.Instance.PlayerControllerMap.ContainsKey(PlayerNumber))
                {
                    Controller = MultiControllerManager.Instance.Controllers[MultiControllerManager.Instance.PlayerControllerMap[PlayerNumber]];
                }
            }
        }
        else
        {
            if (Controller == null)
            {
                if (MultiControllerManager.Instance.PlayerControllerMap.ContainsKey(PlayerNumber))
                {
                    Controller = MultiControllerManager.Instance.Controllers[MultiControllerManager.Instance.PlayerControllerMap[PlayerNumber]];
                }
            }

            PlayerController.SimulateController_Local();
            Duck.Body.SimulateController();
        }
    }

    public void GetRing(CostumeType costumeType)
    {
        StartCoroutine(Co_PlayGenerateRingSound());
        HasRing = true;
        PlayerCostume.Initialize(PlayerNumber, TeamNumber, CostumeType);
        Duck.Ring.Initialize(TeamNumber, costumeType);
        Duck.Ring.GetRing();
        Duck.Wings.GetRing();
    }

    IEnumerator Co_PlayGenerateRingSound()
    {
        yield return new WaitForSeconds(0.4f);
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.DuckGenerateBuoy, Duck.gameObject);
    }

    public void LoseRing(bool explode)
    {
        if (HasRing)
        {
            if (explode)
            {
                Duck.DuckUI.ShowAnnoyingUI();
                AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyPop, Duck.gameObject);
                AudioManager.Instance.SoundPlay("sfx/Sound_Score");
                Goalie.ParticleRelease();
                Duck.Wings.Hit();
            }

            Duck.Ring.LoseRing();
            Duck.Wings.LoseRing();
        }

        HasRing = false;
        Goalie.IsGoalie = false;
    }

    public Vector3 GetPlayerPosition => Duck.Feet.transform.position;

    public void SetPlayerPosition(Vector3 pos)
    {
        Duck.Feet.transform.position = pos;
    }

    public void Reset()
    {
        Duck.DuckRigidbody.velocity = Vector3.zero;
        Duck.DuckRigidbody.angularVelocity = Vector3.zero;
        Duck.Wings.Hit();
        Duck.Ring.LoseRing();
        Duck.Wings.LoseRing();
        Goalie.IsGoalie = false;
    }
}