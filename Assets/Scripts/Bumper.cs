using UnityEngine;

public class Bumper : MonoBehaviour
{
    public Animator Anim;

    public Transform Root;

    public float KickRadius = 1.5f;
    public float Force = 100f;

    public void Kick()
    {
        Anim.SetTrigger("Kick");

        IKickable ko = GameManager.Instance.Ball;
        Vector3 diff = ko.GetRigidbody().transform.position - Root.position;
        float distance = diff.magnitude;
        if (distance < KickRadius)
        {
            ko.GetRigidbody().AddForce(diff.normalized * Force);
        }
    }
}