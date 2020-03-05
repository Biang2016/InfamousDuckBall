using UnityEngine;

public class PlayerMove : Controllable
{
    [SerializeField] private float BrakeVelocityThreshold = 0.1f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        Vector3 diff = Vector3.zero;
        diff += Vector3.forward * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_H];
        diff += Vector3.right * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_V];
        diff = diff.normalized * ParentPlayerControl.Player.Accelerate;

        ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.AddForce(diff);

        if (diff.magnitude < BrakeVelocityThreshold)
        {
        }
    }

    protected override void Operate_AI()
    {
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.velocity.magnitude < BrakeVelocityThreshold)
        {
            ParentPlayerControl.Player.PlayerControl.PlayerRigidbody.velocity *= 0.9f;
        }
    }
}