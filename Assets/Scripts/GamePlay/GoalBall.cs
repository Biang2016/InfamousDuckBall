using System.Collections;
using UnityEngine;

public class GoalBall : MonoBehaviour, IKickable
{
    [SerializeField] public Rigidbody RigidBody;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<GoalCollider>())
        {
            Player p = c.GetComponentInParent<Player>();
            GameManager.Instance.Score(LastKickRobotIndex, p.PlayerInfo.RobotIndex);
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

    internal int LastKickRobotIndex;

    public void Kick(int kickIndex, Vector3 force)
    {
        LastKickRobotIndex = kickIndex;
        RigidBody.AddForce(force);
    }
}