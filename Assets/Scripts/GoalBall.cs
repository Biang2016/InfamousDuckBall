﻿using System.Collections;
using UnityEngine;

public class GoalBall : MonoBehaviour, IKickable
{
    [SerializeField] private Rigidbody RigidBody;

    void OnTriggerEnter(Collider c)
    {
        Player p = c.GetComponentInParent<Player>();
        if (p)
        {
            GameManager.Instance.Score(p.PlayerNumber);
        }

        DeadZone dz = c.GetComponentInParent<DeadZone>();
        if (dz)
        {
            GameManager.Instance.ResetBall();
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
}