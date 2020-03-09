using UnityEngine;
using System.Collections;

public class GooseConfig : MonoBehaviour
{
    [Space] public float Radius = 1f;
    public float MaxSpeed = 2f;
    public float NeckSpeed = 2f;
    public float Accelerate = 2f;

    [Space] public int PushNeckFrame = 5;
    public float PushRadius = 6f;
    public float PushNeckDistance = 3f;
    public float PushForce = 3000f;

    [Space] public int PullNeckFrame = 5;
    public float PullRadius = 10f;
    public float PullNeckDistance = 3f;
    public float PullForce = 3000f;

    [Space] public float HeadRotateSpeed = 5f;
    public float NeckStartTangent = 1f;
    public float NeckEndTangent = 1f;
    public float LookBallAngleThreshold = 45f;
    public float MaxNeckLength = 5f;
}