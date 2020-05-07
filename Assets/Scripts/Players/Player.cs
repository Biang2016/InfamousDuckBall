using System.Collections;
using Bolt;
using UnityEngine;

public class Player : EntityBehaviour<IPlayerState>
{
    public string PlayerName => state.PlayerInfo.PlayerName;
    public PlayerNumber PlayerNumber => (PlayerNumber) state.PlayerInfo.PlayerNumber;
    public TeamNumber TeamNumber => (TeamNumber) state.PlayerInfo.TeamNumber;
    public CostumeType CostumeType => (CostumeType) state.PlayerInfo.CostumeType;

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

    public override void Attached()
    {
        PlayerController.Attached();
        Duck.Attached();
        state.OnUpdateState += OnStateChanged;
    }

    public override void Detached()
    {
        base.Detached();
        Duck.Detached();
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
        state.PlayerInfo.PlayerName = playerName;
        state.PlayerInfo.PlayerNumber = (int) playerNumber;
        state.PlayerInfo.TeamNumber = (int) teamNumber;
        if (entity.IsOwner && !IsInitialized)
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
        if (entity.HasControl && Controller == null)
        {
            if (MultiControllerManager.Instance.PlayerControlMap.ContainsKey(PlayerNumber))
            {
                Controller = MultiControllerManager.Instance.Controllers[MultiControllerManager.Instance.PlayerControlMap[PlayerNumber]];
            }
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