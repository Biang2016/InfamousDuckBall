using Bolt;
using UnityEngine;
using UnityEngine.Events;

public class ScoreRingSingle : EntityBehaviour<IScoreRingSingleState>
{
    [SerializeField] private ScoreRing ScoreRing;
    public Collider Collider;
    public Rigidbody RigidBody;

    internal UnityAction OnRemove;

    private CostumeType costumeType;

    public CostumeType CostumeType
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return (CostumeType) state.CostumeType;
            }
            else
            {
                return costumeType;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.CostumeType = (int) value;
            }
            else
            {
                costumeType = value;
            }
        }
    }

    private TeamNumber teamNumber;

    public TeamNumber TeamNumber
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return (TeamNumber) state.TeamNumber;
            }
            else
            {
                return teamNumber;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.TeamNumber = (int) value;
            }
            else
            {
                teamNumber = value;
            }
        }
    }

    void Update()
    {
        ScoreRing.Initialize(TeamNumber, CostumeType);
    }

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        RigidBody.useGravity = entity.IsOwner;
        Collider.enabled = entity.IsOwner;
    }

    void Awake()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local)
        {
            RigidBody.useGravity = true;
            Collider.enabled = true;
        }
    }

    public void Explode(bool sound)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            BoltNetwork.Destroy(gameObject);
            OnRemove?.Invoke();
        }
    }

    public override void Detached()
    {
        base.Detached();
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyPop, gameObject);
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
    }

    void OnDestroy()
    {
        AudioDuck.Instance?.PlaySound(AudioDuck.Instance.BuoyPop, gameObject);
        FXManager.Instance?.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
    }
}