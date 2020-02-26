using UnityEngine;

public interface IKickable
{
    Rigidbody GetRigidbody();
    void Kick(PlayerNumber playerNumber, Vector3 force);
}