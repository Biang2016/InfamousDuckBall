﻿using Bolt;
using UnityEngine;

public class ScoreRingSingleBoatMenu : MonoBehaviour
{
    [SerializeField] private ScoreRing ScoreRing;
    public Rigidbody RigidBody;

    void Start()
    {
        ScoreRing.Initialize((TeamNumber) Random.Range(0, 2), (CostumeType) Random.Range(0, 3));
    }

    public void Hover()
    {
        RigidBody.AddTorque(Vector3.Scale(Random.insideUnitSphere, new Vector3(1, 0, 1)) * 2000);
    }

    public void Recover()
    {
        ScoreRing.Renderer.enabled = true;
        ScoreRing.Initialize((TeamNumber) Random.Range(0, 2), (CostumeType) Random.Range(0, 3));
    }

    public void Explode()
    {
        ScoreRing.Renderer.enabled = false;
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyPop, gameObject);
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(0, 1, 0));
    }
}