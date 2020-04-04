using UnityEngine;

public class Feet : GooseBodyPart
{
    [SerializeField] private float BrakeVelocityThreshold = 0.1f;
    internal float MoveSpeedModifier = 1f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        Vector3 diff = Vector3.zero;
        diff += Vector3.forward * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_H];
        diff += Vector3.right * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_V];
        diff = diff.normalized * (GooseConfig.Accelerate * MoveSpeedModifier);

        ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.AddForce(diff);

        if (diff.magnitude < BrakeVelocityThreshold)
        {
        }
    }

    protected override void Operate_AI()
    {
    }

    [SerializeField] private Transform LeftFeet;
    [SerializeField] private Transform RightFeet;
    [SerializeField] private Animator FeetAnimator;
    [SerializeField] private float feetMoveThreshold = 1f;

    public enum MoveStates
    {
        Static,
        Forward,
        Backward,
    }

    private MoveStates _moveState;

    private MoveStates MoveState
    {
        get { return _moveState; }
        set
        {
            if (_moveState != value)
            {
                switch (value)
                {
                    case MoveStates.Static:
                    {
                        Goose.Body.BodyAnimator.SetFloat("Gasp", 1.0f);
                        FeetAnimator.SetTrigger("Static");
                        break;
                    }
                    case MoveStates.Forward:
                    {
                        Goose.Body.BodyAnimator.SetFloat("Gasp", 0f);

                        FeetAnimator.SetTrigger("Forward");
                        break;
                    }
                    case MoveStates.Backward:
                    {
                        Goose.Body.BodyAnimator.SetFloat("Gasp", 0f);
                        FeetAnimator.SetTrigger("Backward");
                        break;
                    }
                }

                _moveState = value;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.velocity.magnitude < BrakeVelocityThreshold)
        {
            ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.velocity *= 0.9f;
        }

        Vector3 vel = ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.velocity;
        float forward = Vector3.Dot(vel, Goose.Body.BodyRotate.transform.forward);
        if (forward > feetMoveThreshold)
        {
            Goose.Body.BodyAnimator.SetFloat("Breath", 0.1f);
            Goose.Body.BodyAnimator.SetFloat("Walk", 1.0f);
            MoveState = MoveStates.Forward;
            LeftFeet.right = vel;
            RightFeet.right = vel;
        }
        else if (forward < -feetMoveThreshold)
        {
            Goose.Body.BodyAnimator.SetFloat("Breath", 0.1f);
            Goose.Body.BodyAnimator.SetFloat("Walk", 1.0f);
            MoveState = MoveStates.Backward;
            LeftFeet.right = -vel;
            RightFeet.right = -vel;
        }
        else
        {
            Goose.Body.BodyAnimator.SetFloat("Walk", 0f);
            MoveState = MoveStates.Static;
        }
    }
}