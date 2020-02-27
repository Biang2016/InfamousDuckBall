using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerNumber PlayerNumber;

    public PlayerControl PlayerControl;
    public PlayerCostume PlayerCostume;

    internal int Score = 0;

    public void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Initialize(PlayerNumber playerNumber)
    {
        PlayerNumber = playerNumber;
        PlayerCostume.Initialize(playerNumber);
        PlayerControl.Initialize(this);
    }

    public bool ConsiderPlayerInCamera;

    public void Reviving(bool considerInCamera)
    {
        ConsiderPlayerInCamera = considerInCamera;
        PlayerControl.Controllable = true;
    }

    public Vector3 GetPlayerPosition => transform.position;

    public ParticleSystem ParticleSystem;

    public void Reset()
    {
    }
}

public enum PlayerNumber
{
    Player1 = 0,
    Player2 = 1,
    AnyPlayer = 16,
}