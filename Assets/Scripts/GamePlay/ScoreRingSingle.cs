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
            OnRemove?.Invoke();
        }
    }

    public override void Detached()
    {
        base.Detached();
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyPop, gameObject);
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
    }
}