using UnityEngine;

public interface IKickable
{
    Rigidbody GetRigidbody();
    void Kick(int kickIndex, Vector3 force);
}