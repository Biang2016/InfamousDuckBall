using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public Player Player;
    public PlayerMove PlayerMove;
    public bool Controllable = false;

    public void Initialize(Player player)
    {
        Player = player;
        IPlayerControl[] ipcs = GetComponentsInChildren<IPlayerControl>();
        foreach (IPlayerControl ipc in ipcs)
        {
            ipc.Initialize(this);
        }
    }
}