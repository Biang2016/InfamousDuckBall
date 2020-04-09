using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bolt;

public class Neck : MonoBehaviour
{
    internal Duck Duck;
    internal DuckConfig DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
        DuckConfig = GetComponentInParent<DuckConfig>();
    }

    public Transform HeadPosPivot;
    [SerializeField] private Transform[] Bones;
    [SerializeField] private BezierCurve NeckCurve;

    public Vector3 NeckTargetPos = Vector3.zero;

    public void NeckDeform()
    {
        NeckCurve.BezierInfo.StartPoint = Bones[0].position;
        NeckCurve.BezierInfo.EndPoint = NeckTargetPos;

        float neckLength = (Bones[0].position - NeckTargetPos).magnitude;

        NeckCurve.BezierInfo.StartTangent = Duck.Body.BodyRotate.transform.forward * (DuckConfig.NeckStartTangent * neckLength);
        NeckCurve.BezierInfo.EndTangent = -Duck.Head.transform.forward * (DuckConfig.NeckEndTangent * neckLength);
        List<Vector3> pathPoses = NeckCurve.BezierPath.GetPathPoints(Bones.Length);

        for (int i = 0; i < Bones.Length; i++)
        {
            Bones[i].position = pathPoses[i];
        }

        for (int i = 0; i < Bones.Length - 1; i++)
        {
            Vector3 right = pathPoses[i] - pathPoses[i + 1];
            Quaternion rot = Quaternion.FromToRotation(Bones[i].right, right);
            Bones[i].rotation = rot * Bones[i].rotation;

            Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;
            Quaternion rot_Forward = Quaternion.FromToRotation(Bones[i].forward, forward);
            Bones[i].rotation = rot_Forward * Bones[i].rotation;

            if (i == Bones.Length - 2)
            {
                Bones[i + 1].rotation = rot_Forward * (rot * Bones[i + 1].rotation);
            }
        }
    }
}