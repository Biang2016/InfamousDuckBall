using UnityEngine;
using System.Collections;

public class Neck : GooseBodyPart
{
    public Transform HeadPosPivot;

    [SerializeField] private Transform RootBone;
    private Transform[] Bones;

    void Awake()
    {
        Bones = RootBone.GetComponentsInChildren<Transform>();
    }

    public void MoveNeckTo(Vector3 targetPos)
    {
        Vector3 rootPos = RootBone.position;

        for (int i = 0; i < Bones.Length; i++)
        {
            Vector3 bonePos = Vector3.Lerp(rootPos, targetPos, (float) i / (Bones.Length - 1));
            Bones[i].position = bonePos;
        }
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
    }

    protected override void Operate_AI()
    {
    }
}