using Bolt;
using UnityEngine;
using UnityEngine.Events;

public class ScoreRingSingle : EntityBehaviour<IScoreRingSingleState>
{
    [SerializeField] private ScoreRing ScoreRing;
    public Collider Collider;
    public Rigidbody RigidBody;

    internal UnityAction OnRemove;

    void Update()
    {
        ScoreRing.Initialize((TeamNumber) state.TeamNumber, (CostumeType) state.CostumeType);
    }

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        RigidBody.useGravity = entity.IsOwner;
        Collider.enabled = entity.IsOwner;
    }

    public void Explode(bool sound)
    {
        if (BoltNetwork.IsServer)
        {
            BoltNetwork.Destroy(gameObject);
            if (sound)
            {
                SFX_Event evnt = SFX_Event.Create();
                evnt.SoundName = "BuoyPop";
                evnt.Send();
            }

            OnRemove?.Invoke();
        }
    }

    public override void Detached()
    {
        base.Detached();
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
    }
}