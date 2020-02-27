using UnityEngine;

public class PlayerMove : Controllable
{
    [SerializeField] private float MoveSpeed = 0.3f;

    internal Vector3 PlayerMoveVelocity;
    internal Vector3 lastPosition;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        Vector3 diff = Vector3.zero;
        diff += Vector3.forward * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_H];
        diff += Vector3.right * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.LeftStick_V];
        diff = diff.normalized * MoveSpeed;
        Vector3 tarPos = transform.position + diff;

        tarPos = ParentPlayerControl.Player.TryToMove(tarPos, GameManager.Instance.PlayerRadius);
        lastPosition = transform.position;
        transform.position = tarPos;
        PlayerMoveVelocity = transform.position - lastPosition;
    }

    protected override void Operate_AI()
    {
    }
}