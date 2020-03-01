using UnityEngine;

public class ArmEnd_Clamp : ArmEnd
{
    [SerializeField] private float LookAtMinDistance = 3f;
    [SerializeField] private Animator Anim;
    [SerializeField] private BallInsideCheckTrigger BallInsideCheckTrigger;

    public float ResponseSpeed = 0.5f;
    public float KickForce = 200f;

    public bool IsClamping = false;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            Anim.SetTrigger("Clamp");
            Anim.ResetTrigger("Release");
            BallInsideCheckTrigger.enabled = true;
        }

        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.RightTrigger])
        {
            Anim.SetTrigger("Release");
            Anim.ResetTrigger("Clamp");
        }
    }

    private bool Hold = false;

    protected override void Operate_AI()
    {
    }

    void LateUpdate()
    {
        Quaternion targetRot = new Quaternion();

        if (IsClamping)
        {
            targetRot = Quaternion.LookRotation(transform.position - ParentPlayerControl.Player.GetPlayerPosition);
        }
        else
        {
            targetRot = Quaternion.LookRotation(GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position);
        }

        Quaternion r = Quaternion.Lerp(transform.rotation, targetRot, ResponseSpeed);
        transform.rotation = Quaternion.Euler(Vector3.Scale(new Vector3(0, 1, 1), r.eulerAngles));
    }

    public void OnHold()
    {
        IsClamping = true;
    }

    public void OnRelease()
    {
        IsClamping = false;
        GoalBall ball = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = ball.transform.position - ParentPlayerControl.Player.GetPlayerPosition;
        if (BallInsideCheckTrigger.BallInside)
        {
            Vector3 force = Vector3.Scale(new Vector3(1, 0, 1), diff).normalized * KickForce;
            ball.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, force);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }

        BallInsideCheckTrigger.enabled = false;
    }
}