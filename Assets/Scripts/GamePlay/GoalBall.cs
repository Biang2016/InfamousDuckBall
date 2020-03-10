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
            GameManager.Instance.Score(LastKickTeam, p.PlayerInfo.TeamNumber);
            p.ParticleSystem.Play();
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

    internal TeamNumber LastKickTeam;

    public void Kick(TeamNumber teamNumber, Vector3 force)
    {
        LastKickTeam = teamNumber;
        RigidBody.AddForce(force);
    }
}