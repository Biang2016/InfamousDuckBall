using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    internal Player Player;
    [SerializeField] private Collider Collider;

    void Awake()
    {
    }

    public void Initialize(Player player)
    {
        Player = player;
        string layerName = "PlayerCollider" + ((int) (Player.PlayerInfo.PlayerNumber) + 1);
        Collider.gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}