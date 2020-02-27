using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    private Player Player;

    void Awake()
    {
        Player = GetComponentInParent<Player>();
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        cc.radius = GameManager.Instance.PlayerRadius * 0.98f;
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == GameManager.Instance.Layer_PlayerCollider)
        {
            Player targetPlayer = c.GetComponentInParent<Player>();
            Vector3 relativePos = Player.transform.position - targetPlayer.transform.position;
            if (relativePos.magnitude < 2 * GameManager.Instance.PlayerRadius)
            {
                Vector3 targetVelocity = targetPlayer.PlayerControl.PlayerMove.PlayerMoveVelocity;
                Vector3 myVelocity = Player.PlayerControl.PlayerMove.PlayerMoveVelocity;
                Vector3 relativeVelocity = myVelocity - targetVelocity;
                Vector3 relativeVelocity_Normal = Vector3.Dot(relativeVelocity, relativePos) / relativePos.magnitude * relativePos.normalized;

                Player.transform.position = Player.TryToMove(Player.transform.position - relativeVelocity_Normal / 2, GameManager.Instance.PlayerRadius);
                targetPlayer.transform.position = targetPlayer.TryToMove((targetPlayer.transform.position + relativeVelocity_Normal / 2), GameManager.Instance.PlayerRadius);
            }
        }
    }
}