using UnityEngine;

public class Body : GooseBodyPart
{
    public Transform BodyRotate;
    public Transform StartPivot;

    internal float SpeedModifier = 1f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

        neckTargetPos += Vector3.forward * Goose.NeckSpeed * SpeedModifier * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_H];
        neckTargetPos += Vector3.right * Goose.NeckSpeed * SpeedModifier * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_V];

        float targetRadius = (neckTargetPos - transform.position).magnitude;
        if (targetRadius < Goose.Radius * 2)
        {
            neckTargetPos = (neckTargetPos - transform.position).normalized * Goose.Radius * 2 + transform.position;
        }

        neckTargetPos.y = GameManager.Instance.Cur_BattleManager.Ball.transform.position.y;

        Vector3 diff = neckTargetPos - Goose.Feet.transform.position;
        diff = diff.magnitude > Goose.MaxNeckLength ? diff.normalized * Goose.MaxNeckLength : diff;
        neckTargetPos = diff + Goose.Feet.transform.position;
        MoveNeckTo(neckTargetPos);
    }

    protected override void Operate_AI()
    {
    }

    private Quaternion BodyTargetRot;

    private void MoveNeckTo(Vector3 targetPos)
    {
        Vector3 diff = Vector3.Scale(targetPos - StartPivot.position, new Vector3(1, 0, 1));
        float angleOffset = Vector3.SignedAngle(transform.forward, diff, Vector3.up);
        BodyTargetRot = Quaternion.Euler(new Vector3(0, angleOffset, 0));
        BodyRotate.rotation = BodyTargetRot;
        Goose.Neck.MoveNeckTo(targetPos);
    }

    protected void LateUpdate()
    {
        transform.position = Goose.Feet.transform.position;
        Goose.Head.transform.position = Goose.Neck.HeadPosPivot.position;
    }
}