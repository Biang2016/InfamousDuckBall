using UnityEngine;
using System.Collections;

public abstract class CameraFaceUI : PoolObject
{
    public Transform FollowObject;

    void LateUpdate()
    {
        Vector3 objectScreenPosition = GameManager.Instance.GetCamera().WorldToScreenPoint(FollowObject.position);
        ((RectTransform) transform).position = objectScreenPosition;
    }
}