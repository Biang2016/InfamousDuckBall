﻿using UnityEngine;

public class Head : GooseBodyPart
{
    public override void Initialize(PlayerControl parentPlayerControl)
    {
        base.Initialize(parentPlayerControl);
        foreach (Collider c in transform.GetComponentsInChildren<Collider>())
        {
            if (c.gameObject.layer == GameManager.Instance.Layer_BallKicker)
            {
                string layerName = "BallKicker" + ((int) (ParentPlayerControl.Player.PlayerInfo.PlayerNumber) + 1);
                int layer = LayerMask.NameToLayer(layerName);
                c.gameObject.layer = layer;
            }
        }
    }

    public Animator PushAnim;
    public Animator PullAnim;

    protected override void Operate_AI()
    {
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            Push();
        }
        else
        {
            if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.LeftTrigger])
            {
                Pull();
            }
        }
    }

    private void Push()
    {
        PushAnim.SetTrigger("Kick");
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Goose.KickRadius)
        {
            ko.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, (diff.normalized) * Goose.KickForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    private void Pull()
    {
        PullAnim.SetTrigger("Pull");
    }

    void LateUpdate()
    {
        transform.position = Goose.Neck.HeadPosPivot.position;
        transform.LookAt(GameManager.Instance.Cur_BattleManager.Ball.transform);
    }
}