using UnityEngine;
using System.Collections;
using Bolt;

public class ScoreRingSingle : EntityBehaviour<IScoreRingSingleState>
{
    [SerializeField] private ScoreRing ScoreRing;
    public Collider Collider;
    public Rigidbody RigidBody;

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

    public void Explode()
    {
        if (BoltNetwork.IsServer)
        {
            BoltNetwork.Destroy(gameObject);
        }
    }

    public override void Detached()
    {
        base.Detached();
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
        AudioManager.Instance.SoundPlay("sfx/BalloonPop",0.5f);
    }
}