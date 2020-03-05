using System.Collections.Generic;
using UnityEngine;

public class ArmEnd_Clamp : ArmEnd
{
    [SerializeField] private float LookAtMinDistance = 3f;
    [SerializeField] private InsideClampCheckTrigger InsideClampCheckTrigger;
    [SerializeField] private Animator Anim;
    [SerializeField] private RingSliderAttached RingSliderAttached;
    [SerializeField] private Color HoldColor;
    [SerializeField] private Color CoolDownColor;
    [SerializeField] private float FaceBallPriorityRatio = 0.7f;

    public float ResponseSpeed = 0.5f;
    public float ResponseSpeedRestore = 0.2f;
    public float KickBallForce = 200f;
    public float KickPlayerForce = 200f;

    void Start()
    {
        RingSliderAttached.CameraFaceSlider.SetColor(Color.clear);
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            if (!IsHoldingButton && !InCoolDown)
            {
                Clamp();
            }
        }

        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.RightTrigger])
        {
            if (IsHoldingButton)
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
        IsHoldingButton = true;
    }

    private void Release()
    {
        Anim.SetTrigger("Release");
        Anim.ResetTrigger("Clamp");
        IsHoldingButton = false;
        RemainForceRatio = (HoldMaxDuration - HoldTick) / HoldMaxDuration;
        HoldTick = 0;
    }

    private bool InCoolDown = false;
    private bool IsHoldingButton = false;
    private bool ReleaseByDuration = false;

    protected override void Operate_AI()
    {
    }

    private float HoldTick = 0f;
    [SerializeField] private float HoldMaxDuration = 2f;
    [SerializeField] private float CoolDownTick = 0;
    [SerializeField] private float CoolDownDuration = 0.5f;
    private float RemainForceRatio;

    void Update()
    {
        if (IsHoldingButton)
        {
            if (InsideClampCheckTrigger.IsPlayerInside)
            {
                Arm.SpeedModifier = 0.4f;
                ParentPlayerControl.PlayerMove.MoveSpeedModifier = 0.1f;
            }
            else
            {
                Arm.SpeedModifier = 1f;
                ParentPlayerControl.PlayerMove.MoveSpeedModifier = 1f;
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
        else
        {
            Arm.SpeedModifier = 1f;
            ParentPlayerControl.PlayerMove.MoveSpeedModifier = 1f;
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

    public bool IsClamped = false;

    void LateUpdate()
    {
        Quaternion targetRot = new Quaternion();

        Vector3 lookAtTarget = Vector3.zero;

        float minDist = 9999f;

        foreach (KeyValuePair<PlayerNumber, Player> kv in GameManager.Instance.PlayerDict)
        {
            if (kv.Key != ParentPlayerControl.Player.PlayerInfo.PlayerNumber)
            {
                float dist = (transform.position - kv.Value.GetPlayerPosition).magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    lookAtTarget = kv.Value.GetPlayerPosition;
                }
            }
        }

        if (!InsideClampCheckTrigger.IsBallInside)
        {
            float _dist = (transform.position - GameManager.Instance.Cur_BattleManager.Ball.transform.position).magnitude * FaceBallPriorityRatio;
            if (_dist < minDist)
            {
                minDist = _dist;
                lookAtTarget = GameManager.Instance.Cur_BattleManager.Ball.transform.position;
            }
        }

        targetRot = Quaternion.LookRotation(lookAtTarget - transform.position);
        Quaternion r = Quaternion.Lerp(transform.rotation, targetRot, IsClamped ? ResponseSpeed : ResponseSpeedRestore);
        transform.rotation = Quaternion.Euler(Vector3.Scale(new Vector3(0, 1, 0), r.eulerAngles));
    }

    public void OnHold()
    {
        IsClamped = true;
    }

    public void OnRelease()
    {
        IsClamped = false;
        if (!ReleaseByDuration)
        {
            if (InsideClampCheckTrigger.IsBallInside)
            {
                GoalBall ball = InsideClampCheckTrigger.InsideBall;
                if (ball)
                {
                    Vector3 force = transform.forward * KickBallForce * RemainForceRatio;
                    ball.Kick(ParentPlayerControl.Player.PlayerInfo.RobotIndex, force);
                    FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, ball.transform.position, Quaternion.FromToRotation(Vector3.back, transform.forward));
                }
            }

            if (InsideClampCheckTrigger.IsPlayerInside)
            {
                Player player = InsideClampCheckTrigger.InsidePlayer;
                if (player)
                {
                    Vector3 force = transform.forward * KickPlayerForce * RemainForceRatio;
                    player.PlayerControl.PlayerRigidbody.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        InCoolDown = true;
        CoolDownTick = 0;
        InsideClampCheckTrigger.enabled = false;
    }
}