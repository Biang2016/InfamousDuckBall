using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerNumber PlayerNumber;
    public int Score = 0;

    public void Initialize()
    {
        Spring[] springs = GetComponentsInChildren<Spring>();
        foreach (Spring sps in springs)
        {
            sps.PlayerNumber = PlayerNumber;
        }
    }

    public RotateArm ArmRotate;
    public RotateArm Arm1;
    public RotateArm Arm2;
    public Transform HammerRoot;

    [SerializeField] private float MoveSpeed = 0.3f;
    [SerializeField] private JoystickAxis HorizontalAxis = JoystickAxis.None;
    [SerializeField] private JoystickAxis VerticalAxis = JoystickAxis.None;
    [SerializeField] private float ArmSpeed;
    [SerializeField] private JoystickAxis ArmHorizontalAxis = JoystickAxis.None;
    [SerializeField] private JoystickAxis ArmVerticalAxis = JoystickAxis.None;

    public ParticleSystem ParticleSystem;

    void FixedUpdate()
    {
        MoveArm();
        Move();
    }

    private void MoveArm()
    {
        float hor = Input.GetAxis(ArmHorizontalAxis + "_" + PlayerNumber);
        Vector3 tarPos = HammerRoot.position;
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
        Vector3 diff = Vector3.Scale(targetPos - transform.position, new Vector3(1, 0, 1));
        float angleOffset = Vector3.SignedAngle(transform.forward, diff, Vector3.up);
        ArmRotate.SetRotation(angleOffset);

        float distance = Mathf.Clamp(diff.magnitude, Mathf.Abs(Arm1.Length - Arm2.Length) + 0.2f, Arm1.Length + Arm2.Length - 0.2f);

        float angle_Arm1 = -Mathf.Rad2Deg * Mathf.Acos((Arm1.Length * Arm1.Length + distance * distance - Arm2.Length * Arm2.Length) / (2 * Arm1.Length * distance));
        Arm1.SetRotation(angle_Arm1);

        float angle_Arm2 = 180 - Mathf.Rad2Deg * Mathf.Acos((Arm1.Length * Arm1.Length + Arm2.Length * Arm2.Length - distance * distance) / (2 * Arm1.Length * Arm2.Length));
        Arm2.SetRotation(angle_Arm2);
    }


    private void Move()
    {
        float hor = Input.GetAxis(HorizontalAxis + "_" + PlayerNumber);
        Vector3 tarPos = Vector3.zero;
        if (Mathf.Abs(hor) > 0.3f)
        {
            tarPos += Vector3.forward * MoveSpeed * hor;
        }

        float ver = Input.GetAxis(VerticalAxis + "_" + PlayerNumber);
        if (Mathf.Abs(ver) > 0.3f)
        {
            tarPos += Vector3.right * MoveSpeed * ver;
        }

        Vector3 tarPosGlobal = transform.TransformPoint(tarPos);
        tarPosGlobal.x = Mathf.Clamp(tarPosGlobal.x, GameManager.Instance.X_Min, GameManager.Instance.X_Max);
        tarPosGlobal.z = Mathf.Clamp(tarPosGlobal.z, GameManager.Instance.Z_Min, GameManager.Instance.Z_Max);

        Player enemyPlayer = null;
        switch (PlayerNumber)
        {
            case PlayerNumber.P1:
            {
                enemyPlayer = GameManager.Instance.Player2;
                break;
            }
            case PlayerNumber.P2:
            {
                enemyPlayer = GameManager.Instance.Player1;
                break;
            }
        }

        Vector3 diff = Vector3.Scale(new Vector3(1, 0, 1), tarPosGlobal - enemyPlayer.transform.position);
        float distance = diff.magnitude;
        if (distance < 2 * GameManager.Instance.PlayerRadius)
        {
            Vector3 offset = diff.normalized * 2 * GameManager.Instance.PlayerRadius;
            tarPosGlobal = enemyPlayer.transform.position + offset;
        }

        transform.position = tarPosGlobal;
    }
}

public enum PlayerNumber
{
    P1 = 0,
    P2 = 1,
    AnyPlayer = 16,
}