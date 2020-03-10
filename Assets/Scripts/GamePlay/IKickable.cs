using UnityEngine;

public interface IKickable
{
    Rigidbody GetRigidbody();
    void Kick(TeamNumber teamNumber, Vector3 force);
}