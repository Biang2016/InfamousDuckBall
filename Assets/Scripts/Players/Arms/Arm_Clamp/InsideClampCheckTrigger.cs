using UnityEngine;
using System.Collections;

public class InsideClampCheckTrigger : MonoBehaviour
{
    public Collider Collider;
    internal bool BallInside = false;
    internal bool PlayerInside = false;

    void OnTriggerStay(Collider c)
    {
        BallInside = c.gameObject.layer == GameManager.Instance.Layer_Ball;
        PlayerInside = GameManager.Instance.IsPlayerColliderLayer(c.gameObject.layer);
    }
}