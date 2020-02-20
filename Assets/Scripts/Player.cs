using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerNumber PlayerNumber;

    public void Initialize()
    {
        RotateArm[] ras = GetComponentsInChildren<RotateArm>();
        foreach (RotateArm rotateArm in ras)
        {
            rotateArm.PlayerNumber = PlayerNumber;
        }

        Spring[] springs = GetComponentsInChildren<Spring>();
        foreach (Spring sps in springs)
        {
            sps.PlayerNumber = PlayerNumber;
        }
    }
}

public enum PlayerNumber
{
    Player1 = 0,
    Player2 = 1,
    AnyPlayer = 16,
}