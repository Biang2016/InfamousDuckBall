using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    internal Player Player;
    internal PlayerMove PlayerMove;
    internal PlayerUpperPart PlayerUpperPart;
    public Rigidbody PlayerRigidbody;
    public bool Controllable = false;

    void Awake()
    {
        Player = GetComponent<Player>();
        PlayerMove = GetComponentInChildren<PlayerMove>();
        PlayerUpperPart = GetComponentInChildren<PlayerUpperPart>();
        PlayerUpperPart.PlayerBase = PlayerMove.transform;
    }

    public void Initialize(Player player)
    {
        Player = player;
        Controllable[] controllables = GetComponentsInChildren<Controllable>();
        foreach (Controllable c in controllables)
        {
            c.Initialize(this);
        }

        PlayerMove.transform.localScale = new Vector3(player.Radius, 1, player.Radius);
    }


    void FixedUpdate()
    {
        if (PlayerRigidbody.velocity.magnitude > Player.MaxSpeed)
        {
            PlayerRigidbody.velocity = PlayerRigidbody.velocity.normalized * Player.MaxSpeed;
        }
    }
}