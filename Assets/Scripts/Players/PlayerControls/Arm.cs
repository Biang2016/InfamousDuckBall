using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;

public class Arm : PlayerUpperPart
{
    public RotateArmSection ArmRotate;
    public RotateArmSection Arm1;
    public RotateArmSection Arm2;
    public Transform StartPivot;
    public Transform ArmEndPivot;
    public ArmEnd ArmEnd;

    [SerializeField] private float ArmSpeed;

    void Awake()
    {
        ArmEnd = GetComponentInChildren<ArmEnd>();
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        Vector3 tarPos = ArmEndPivot.position;

        tarPos += Vector3.forward * ArmSpeed * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_H];
        tarPos += Vector3.right * ArmSpeed * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_V];

        MoveArmTo(tarPos);
    }

    protected override void Operate_AI()
    {
    }

    private void MoveArmTo(Vector3 targetPos)
    {
        Vector3 diff = Vector3.Scale(targetPos - StartPivot.position, new Vector3(1, 0, 1));
        float angleOffset = Vector3.SignedAngle(transform.forward, diff, Vector3.up);
        ArmRotate.SetRotation(angleOffset);

        float distance = Mathf.Clamp(diff.magnitude, Mathf.Abs(Arm1.Length - Arm2.Length) + 0.2f, Arm1.Length + Arm2.Length - 0.2f);

        float angle_Arm1 = -Mathf.Rad2Deg * Mathf.Acos((Arm1.Length * Arm1.Length + distance * distance - Arm2.Length * Arm2.Length) / (2 * Arm1.Length * distance));
        Arm1.SetRotation(angle_Arm1);

        float angle_Arm2 = 180 - Mathf.Rad2Deg * Mathf.Acos((Arm1.Length * Arm1.Length + Arm2.Length * Arm2.Length - distance * distance) / (2 * Arm1.Length * Arm2.Length));
        Arm2.SetRotation(angle_Arm2);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        ArmEnd.transform.position = ArmEndPivot.position;
    }
}