using UnityEngine;

public class ArmEnd_SpringHammer : ArmEnd
{
    public Animator Anim;
    public Animator PullAnim;

    public float KickRadius = 1.5f;
    public float KickForce = 3000f;

    public float PullRadius = 10f;
    public float PullForce = 3000f;

    protected override void Operate_AI()
    {
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            Bump();
        }
        else
        {
            if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.LeftTrigger])
            {
                Pull();
            }
        }
    }

    private void Bump()
    {
        Anim.SetTrigger("Kick");
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < KickRadius)
        {
            ko.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, (diff.normalized) * KickForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    private void Pull()
    {
        PullAnim.SetTrigger("Pull");
    }

    void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.Cur_BattleManager.Ball.transform);
    }
}