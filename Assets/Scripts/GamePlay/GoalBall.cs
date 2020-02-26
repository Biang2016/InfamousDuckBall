using System.Collections;
using UnityEngine;

public class GoalBall : MonoBehaviour, IKickable
{
    [SerializeField] private Rigidbody RigidBody;

    void OnTriggerEnter(Collider c)
    {
        Player p = c.GetComponentInParent<Player>();
        if (p)
        {
            GameManager.Instance.Score(LastKickPlayerNumber, p.PlayerNumber);
        }
    }

    public void Reset()
    {
        RigidBody.velocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;
    }

    public Rigidbody GetRigidbody()
    {
        return RigidBody;
    }

    internal PlayerNumber LastKickPlayerNumber;

    public void Kick(PlayerNumber playerNumber, Vector3 force)
    {
        LastKickPlayerNumber = playerNumber;
        RigidBody.AddForce(force);
    }
}