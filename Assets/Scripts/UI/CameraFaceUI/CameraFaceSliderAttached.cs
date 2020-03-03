using System;
using UnityEngine;

public class CameraFaceSliderAttached<T> : MonoBehaviour where T : CameraFaceSlider
{
    internal CameraFaceSlider CameraFaceSlider;

    void Awake()
    {
        CameraFaceSlider = GameObjectPoolManager.Instance.PoolDict[(GameObjectPoolManager.PrefabNames) Enum.Parse(typeof(GameObjectPoolManager.PrefabNames), typeof(T).Name)].AllocateGameObject<RingSlider>(WorldUIManager.Instance.Canvas.transform);
        CameraFaceSlider.FollowObject = transform;
    }
}