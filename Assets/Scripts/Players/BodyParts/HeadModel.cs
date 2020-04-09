﻿using UnityEngine;
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
        Ball ball = GameManager.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Head.DuckConfig.PullRadius)
        {
            ball.RigidBody.DOMove(Head.transform.position + Head.transform.forward * Head.DuckConfig.PullBallStopFromHead, Head.DuckConfig.PullDuration);
            ball.Kick(Head.Duck.Player.TeamNumber, (-diff.normalized) * 0);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    public void OnPush()
    {
        Head.Duck.Body.BodyAnimator.SetFloat("Tail", 1.0f);
        Head.Duck.Body.BodyAnimator.SetFloat("Breath", 0.0f);
        AudioManager.Instance.SoundPlay("sfx/Sound_Push");
        Head.Duck.Body.PushNeck();
        Ball ball = GameManager.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Head.DuckConfig.PushRadius + Head.Duck.Body.ChargeDistance)
        {
            ball.Kick(Head.Duck.Player.TeamNumber, (Head.transform.forward) * (Head.DuckConfig.PushForce + Head.PushChargeForceRatio * Head.DuckConfig.PushChargingExtraForce));
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    public void OnPushOver()
    {
        Head.HeadStatus = Head.HeadStatusTypes.Idle;
        Head.HeadCollider.enabled = true;
        Head.Anim.ResetTrigger("Push");
        Head.Anim.ResetTrigger("Pull");
    }

    public void OnPullOver()
    {
        Head.HeadStatus = Head.HeadStatusTypes.Idle;
        Head.HeadCollider.enabled = true;
        Head.Anim.ResetTrigger("Push");
        Head.Anim.ResetTrigger("Pull");
    }
}