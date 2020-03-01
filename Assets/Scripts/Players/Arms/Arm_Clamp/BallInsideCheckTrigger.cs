using UnityEngine;
using System.Collections;

public class BallInsideCheckTrigger : MonoBehaviour
{
    public Collider Collider;
    internal bool BallInside = false;

    public void OnDisable()
    {
        Collider.enabled = false;
        BallInside = false;
    }

    public void OnEnable()
    {
        Collider.enabled = true;
        BallInside = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == GameManager.Instance.Layer_Ball)
        {
            BallInside = true;
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == GameManager.Instance.Layer_Ball)
        {
            BallInside = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == GameManager.Instance.Layer_Ball)
        {
            BallInside = false;
        }
    }
}