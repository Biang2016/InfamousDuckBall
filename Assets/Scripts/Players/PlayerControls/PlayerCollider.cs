using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    private Player Player;

    void Awake()
    {
        Player = GetComponentInParent<Player>();
    }
}