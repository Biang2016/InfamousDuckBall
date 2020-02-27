using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    internal Player Player;
    internal PlayerMove PlayerMove;
    public Rigidbody PlayerRigidbody;
    public bool Controllable = false;

    void Awake()
    {
        Player = GetComponent<Player>();
        PlayerMove = GetComponent<PlayerMove>();
    }

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