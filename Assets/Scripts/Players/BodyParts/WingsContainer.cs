using UnityEngine;
using System.Collections;

public class WingsContainer : MonoBehaviour
{
    [SerializeField] private Transform LeftWingJoint;
    [SerializeField] private Transform RightWingJoint;

    private Quaternion InitRotation;

    void Awake()
    {
        InitRotation = LeftWingJoint.rotation;
    }

    void LateUpdate()
    {
        Vector3 pos = (LeftWingJoint.position + RightWingJoint.position) / 2;
        transform.position = pos;

        Quaternion rot = LeftWingJoint.rotation * Quaternion.Inverse(InitRotation);
        transform.rotation = rot;

    }
}