﻿using UnityEngine;

public class HydraulicHammer : MonoBehaviour, IPlayerControl
{
    private PlayerControl ParentPlayerControl;

    public void Initialize(PlayerControl parentPlayerControl)
    {
        ParentPlayerControl = parentPlayerControl;
    }

    public Animator Anim;

    void FixedUpdate()
    {
        if (MultiControllerManager.Instance.PlayerControlMap.ContainsKey(ParentPlayerControl.Player.PlayerInfo.PlayerNumber))
        {
            PlayerNumber myControllerIndex = MultiControllerManager.Instance.PlayerControlMap[ParentPlayerControl.Player.PlayerInfo.PlayerNumber];

            if (ParentPlayerControl && ParentPlayerControl.Controllable)
            {
                if (MultiControllerManager.Instance.Controllers[myControllerIndex].ButtonDown[ControlButtons.RightTrigger])
                {
                    Bump();
                }
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.Cur_BattleManager.Ball.transform);
    }

    public float KickRadius = 1.5f;
    public float Force = 100f;

    public void Bump()
    {
        Anim.SetTrigger("Kick");
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;
        float distance = diff.magnitude;
        if (distance < KickRadius)
        {
            ko.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, (diff.normalized) * Force);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }
}