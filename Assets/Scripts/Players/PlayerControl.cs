using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    internal Player Player;
    internal Goose Goose;
    internal GooseConfig GooseConfig;
    internal PlayerCollider PlayerCollider;
   
    public Rigidbody PlayerRigidbody;
    public bool Controllable = false;

    void Awake()
    {
        Player = GetComponent<Player>();
        Goose = GetComponent<Goose>();
        GooseConfig = GetComponent<GooseConfig>();
        PlayerCollider = GetComponentInChildren<PlayerCollider>();
    }

    public void Initialize(Player player)
    {
        Player = player;
        Controllable[] controllables = GetComponentsInChildren<Controllable>();
        foreach (Controllable c in controllables)
        {
            c.Initialize(this);
        }

        PlayerCollider.Initialize(player);
    }

    void FixedUpdate()
    {
        if (PlayerRigidbody.velocity.magnitude > GooseConfig.MaxSpeed)
        {
            PlayerRigidbody.velocity = PlayerRigidbody.velocity.normalized * GooseConfig.MaxSpeed;
        }
    }
}