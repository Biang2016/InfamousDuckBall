using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerNumber PlayerNumber;
    internal int Score = 0;

    public void Initialize()
    {
        //IPlayerControl[] ipcs_self = GetComponents<IPlayerControl>();
        //foreach (IPlayerControl ipc in ipcs_self)
        //{
        //    ipc.SetPlayerNumber(PlayerNumber);
        //}

        IPlayerControl[] ipcs = GetComponentsInChildren<IPlayerControl>();
        foreach (IPlayerControl ipc in ipcs)
        {
            ipc.SetPlayerNumber(PlayerNumber);
        }
    }

    public ParticleSystem ParticleSystem;
}

public enum PlayerNumber
{
    P1 = 0,
    P2 = 1,
    AnyPlayer = 16,
}