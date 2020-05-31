using DG.Tweening;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class HeadModel : MonoBehaviour
{
    [SerializeField] private Head Head;

    public void OnPull()
    {
        Head.Duck.Body.BodyAnimator.SetFloat("Tail", 1.0f);
        Head.Duck.Body.BodyAnimator.SetFloat("Breath", 0.0f);

        AudioDuck.Instance.StartPlayerQuackSound(Head.Player.PlayerNumber, (float) Head.Player.TeamNumber, 1, transform, Head.Duck.DuckRigidbody);
        Head.Duck.Body.PullNeck();
        Head.Duck.Wings.Kick();

        Ball ball = GameManager.Instance.GetBallByHeadPos(transform.position);
        if (ball)
        {
            Vector3 diff = ball.transform.position - transform.position;
            float distance = diff.magnitude;
            if (distance < Head.DuckConfig.PullRadius * ConfigManager.Instance.DuckConfiguration_Multiplier.PullRadiusMulti)
            {
                ball.RigidBody.velocity = Vector3.zero;
                ball.RigidBody.angularVelocity = Vector3.zero;
                ball.RigidBody.DOMove(Head.transform.position + Head.transform.forward * Head.DuckConfig.PullBallStopFromHead, Head.DuckConfig.PullDuration);
                AudioDuck.Instance.PlaySound(AudioDuck.Instance.FishBreath, ball.gameObject);
                FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
            }
        }
    }

    public void OnPush()
    {
        Head.Duck.Body.BodyAnimator.SetFloat("Tail", 1.0f);
        Head.Duck.Body.BodyAnimator.SetFloat("Breath", 0.0f);
        AudioDuck.Instance.StartPlayerQuackSound(Head.Player.PlayerNumber, (float) Head.Player.TeamNumber, 0, transform, Head.Duck.DuckRigidbody);
        //if (Head.Player.entity.HasControl)
        //{
        //    AudioManager.Instance.SoundPlay("sfx/Sound_Push", 1f);
        //}
        //else
        //{
        //    AudioManager.Instance.SoundPlay("sfx/Sound_Push", 0.5f);
        //}

        Head.Duck.Body.PushNeck();
        Head.Duck.Wings.Kick();

        Ball ball = GameManager.Instance.GetBallByHeadPos(transform.position);
        if (ball)
        {
            Vector3 diff = ball.transform.position - transform.position;
            float distance = diff.magnitude;
            if (distance < Head.DuckConfig.PushRadius * ConfigManager.Instance.DuckConfiguration_Multiplier.PushRadiusMulti + Head.Duck.Body.ChargeDistance)
            {
                AudioDuck.Instance.PlaySound(AudioDuck.Instance.FishBreath, ball.gameObject);
                ball.Kick(Head.Duck.Player.TeamNumber, (Head.transform.forward) * (Head.DuckConfig.PushForce + Head.PushChargeForceRatio * Head.DuckConfig.PushChargingExtraForce));
                FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
            }
        }
    }

    public void OnPushOver()
    {
        Head.HeadStatus = Head.HeadStatusTypes.Idle;
        Head.Anim.ResetTrigger("Push");
        Head.Anim.ResetTrigger("Pull");
    }

    public void OnPullOver()
    {
        Head.HeadStatus = Head.HeadStatusTypes.Idle;
        Head.Anim.ResetTrigger("Push");
        Head.Anim.ResetTrigger("Pull");
    }
}