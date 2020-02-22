using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour, IPlayerControl
{
    private PlayerNumber PlayerNumber;

    public void SetPlayerNumber(PlayerNumber playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    [SerializeField] private float ArmSpeed;
    [SerializeField] private JoystickAxis ArmHorizontalAxis = JoystickAxis.None;
    [SerializeField] private JoystickAxis ArmVerticalAxis = JoystickAxis.None;

    public RotateArmSection ArmRotate;
    public RotateArmSection Arm1;
    public RotateArmSection Arm2;
    public Transform StartPivot;
    public Transform TerminalPivot;

    void FixedUpdate()
    {
        MoveArm();
    }

    private void MoveArm()
    {
        float hor = Input.GetAxis(ArmHorizontalAxis + "_" + PlayerNumber);
        Vector3 tarPos = TerminalPivot.position;
        if (Mathf.Abs(hor) > 0.3f)
        {
            tarPos += Vector3.forward * ArmSpeed * hor;
        }

        float ver = Input.GetAxis(ArmVerticalAxis + "_" + PlayerNumber);
        if (Mathf.Abs(ver) > 0.3f)
        {
            tarPos += Vector3.right * ArmSpeed * ver;
        }

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