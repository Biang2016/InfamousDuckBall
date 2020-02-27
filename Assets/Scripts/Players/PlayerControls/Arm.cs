using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour, IPlayerControl
{
    private PlayerControl ParentPlayerControl;

    public void Initialize(PlayerControl parentPlayerControl)
    {
        ParentPlayerControl = parentPlayerControl;
    }

    public RotateArmSection ArmRotate;
    public RotateArmSection Arm1;
    public RotateArmSection Arm2;
    public Transform StartPivot;
    public Transform TerminalPivot;

    void FixedUpdate()
    {
        if (ParentPlayerControl && ParentPlayerControl.Controllable)
        {
            MoveArm();
        }
    }

    [SerializeField] private float ArmSpeed;

    private void MoveArm()
    {
        if (!MultiControllerManager.Instance.PlayerControlMap.ContainsKey(ParentPlayerControl.Player.PlayerInfo.PlayerNumber)) return;
        PlayerNumber myControllerIndex = MultiControllerManager.Instance.PlayerControlMap[ParentPlayerControl.Player.PlayerInfo.PlayerNumber];

        Vector3 tarPos = TerminalPivot.position;

        tarPos += Vector3.forward * ArmSpeed * MultiControllerManager.Instance.Controllers[myControllerIndex].Axises[ControlAxis.RightStick_H];
        tarPos += Vector3.right * ArmSpeed * MultiControllerManager.Instance.Controllers[myControllerIndex].Axises[ControlAxis.RightStick_V];

        MoveArmTo(tarPos);
    }

    public void MoveArmTo(Vector3 targetPos)
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
}