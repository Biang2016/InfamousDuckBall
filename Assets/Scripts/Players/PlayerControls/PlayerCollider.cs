using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    private CapsuleCollider CapsuleCollider;

    void Awake()
    {
        CapsuleCollider = GetComponent<CapsuleCollider>();
        CapsuleCollider.radius = GameManager.Instance.PlayerRadius * 0.98f;
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == GameManager.Instance.Layer_PlayerCollider)
        {
            Player player = c.GetComponentInParent<Player>();
            player.transform.position = player.TryToMove((c.transform.position - transform.position).normalized * 2 * GameManager.Instance.PlayerRadius + transform.position, GameManager.Instance.PlayerRadius);
        }
    }
}