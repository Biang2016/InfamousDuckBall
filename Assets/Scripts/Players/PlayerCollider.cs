using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    internal Player Player;
    [SerializeField] private Collider Collider;

    public void Initialize(Player player)
    {
        Player = player;
        string layerName = "PlayerCollider" + ((int) (Player.PlayerNumber) + 1);
        Collider.gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}