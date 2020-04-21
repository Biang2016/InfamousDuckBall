using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HeadModel : MonoBehaviour
{
    [SerializeField] private Head Head;

    public void OnPull()
    {
        Head.Duck.Body.BodyAnimator.SetFloat("Tail", 1.0f);
        Head.Duck.Body.BodyAnimator.SetFloat("Breath", 0.0f);
        AudioManager.Instance.SoundPlay("sfx/Sound_Pull");
        Head.Duck.Body.PullNeck();
        Head.Duck.Wings.Kick();

            Ball ball = GameManager.Instance.Ball;
            if (ball)
            {
                Vector3 diff = ball.transform.position - transform.position;
                float distance = diff.magnitude;
                if (distance < Head.DuckConfig.PullRadius * GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti)
                {
                    ball.RigidBody.DOMove(Head.transform.position + Head.transform.forward * Head.DuckConfig.PullBallStopFromHead, Head.DuckConfig.PullDuration);
                    ball.Kick(Head.Duck.Player.TeamNumber, (-diff.normalized) * 0);
                    FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
                }
            }
    }

    public void OnPush()
    {
        Head.Duck.Body.BodyAnimator.SetFloat("Tail", 1.0f);
        Head.Duck.Body.BodyAnimator.SetFloat("Breath", 0.0f);
        AudioManager.Instance.SoundPlay("sfx/Sound_Push");
        Head.Duck.Body.PushNeck();
        Head.Duck.Wings.Kick();

            Ball ball = GameManager.Instance.Ball;
            if (ball)
            {
                Vector3 diff = ball.transform.position - transform.position;
                float distance = diff.magnitude;
                if (distance < Head.DuckConfig.PushRadius * GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti + Head.Duck.Body.ChargeDistance)
                {
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