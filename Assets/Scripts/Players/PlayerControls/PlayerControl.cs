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
        Controllable[] controllables = GetComponentsInChildren<Controllable>();
        foreach (Controllable c in controllables)
        {
            c.Initialize(this);
        }
    }
}