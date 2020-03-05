using UnityEngine;

public class ArmEnd_Clamp : ArmEnd
{
    [SerializeField] private float LookAtMinDistance = 3f;
    [SerializeField] private InsideClampCheckTrigger InsideClampCheckTrigger;
    [SerializeField] private Animator Anim;
    [SerializeField] private RingSliderAttached RingSliderAttached;
    [SerializeField] private Color HoldColor;
    [SerializeField] private Color CoolDownColor;

    public float ResponseSpeed = 0.5f;
    public float KickForce = 200f;

    void Start()
    {
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            if (!Hold && !InCoolDown)
            {
                Clamp();
            }
        }

        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.RightTrigger])
        {
            if (Hold)
            {
                ReleaseByDuration = false;
                Release();
            }
        }
    }

    private void Clamp()
    {
        Anim.SetTrigger("Clamp");
        Anim.ResetTrigger("Release");
        Hold = true;
    }

    private void Release()
    {
        Anim.SetTrigger("Release");
        Anim.ResetTrigger("Clamp");
        Hold = false;
        HoldTick = 0;
    }

    private bool InCoolDown = false;
    private bool Hold = false;
    private bool ReleaseByDuration = false;

    protected override void Operate_AI()
    {
    }

    private float HoldTick = 0f;
    [SerializeField] private float HoldMaxDuration = 2f;
    [SerializeField] private float CoolDownTick = 0;
    [SerializeField] private float CoolDownDuration = 0.5f;

    void Update()
    {
        if (Hold)
        {
            if (InsideClampCheckTrigger.PlayerInside)
            {
                Arm.SpeedModifier = 0.4f;
            }
            else
            {
                Arm.SpeedModifier = 1f;
            }

            HoldTick += Time.deltaTime;
            RingSliderAttached.CameraFaceSlider.SetColor(HoldColor);
            RingSliderAttached.CameraFaceSlider.RefreshValue((HoldMaxDuration - HoldTick) / HoldMaxDuration);
            if (HoldTick > HoldMaxDuration)
            {
                ReleaseByDuration = true;
                Release();
            }
        }

        if (InCoolDown)
        {
            CoolDownTick += Time.deltaTime;
            RingSliderAttached.CameraFaceSlider.SetColor(CoolDownColor);
            RingSliderAttached.CameraFaceSlider.RefreshValue((CoolDownDuration - CoolDownTick) / CoolDownDuration);
            if (CoolDownTick > CoolDownDuration)
            {
                InCoolDown = false;
            }
        }
    }

    public bool IsClamping = false;

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
        if (!ReleaseByDuration)
        {
            GoalBall ball = GameManager.Instance.Cur_BattleManager.Ball;
            Vector3 diff = ball.transform.position - ParentPlayerControl.Player.GetPlayerPosition;
            if (InsideClampCheckTrigger.BallInside)
            {
                Vector3 force = Vector3.Scale(new Vector3(1, 0, 1), diff).normalized * KickForce;
                ball.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, force);
                FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
            }
        }

        InCoolDown = true;
        CoolDownTick = 0;
        InsideClampCheckTrigger.enabled = false;
    }
}