using UnityEngine;
using System.Collections;

public class Goose : MonoBehaviour
{
    public Feet Feet;
    public Body Body;
    public Neck Neck;
    public Head Head;


    public float Radius = 1f;
    public float MaxSpeed = 2f;
    public float NeckSpeed = 2f;
    public float Accelerate = 2f;

    public float KickRadius = 1.5f;
    public float KickForce = 3000f;

    public float PullRadius = 10f;
    public float PullForce = 3000f;
}
