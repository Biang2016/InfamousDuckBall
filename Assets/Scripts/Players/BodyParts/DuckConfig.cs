using UnityEngine;

public class DuckConfig : MonoBehaviour
{
    [Space] public float Radius = 1f;
    public float MaxSpeed = 2f;
    public float NeckSpeed = 2f;
    public float Accelerate = 2f;
    public float BrakeVelocityThreshold = 0.3f;

    [Space] public int PushNeckFrame = 5;
    public float PushRadius = 6f;
    public float PushNeckDistance = 3f;
    public float PushForce = 3000f;
    public float PushChargingExtraForce = 3000f;
    public float PushChargeTimeMaxDuration = 1f;

    [Space] public int PullNeckFrame = 5;
    public float PullRadius = 10f;
    public float PullNeckDistance = 3f;
    public float PullBallStopFromHead = 2.5f;
    public float PullDuration = 0.2f;
    public float PullChargeBackward = 0.1f;

    [Space] public float HeadRotateSpeed = 5f;
    public float NeckStartTangent => MaxNeckLength / 18f;
    public float NeckEndTangent => MaxNeckLength / 18f;
    public float LookBallAngleThreshold = 45f;
    public float MinNeckLength = 1f;
    public float MaxNeckLength = 5f;
}