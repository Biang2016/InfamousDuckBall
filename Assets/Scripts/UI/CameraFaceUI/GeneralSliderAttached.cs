using UnityEngine;
using System.Collections;

public class GeneralSliderAttached : MonoBehaviour
{
    internal GeneralSlider GeneralSlider;

    void Awake()
    {
        GeneralSlider = GameObjectPoolManager.Instance.PoolDict[GameObjectPoolManager.PrefabNames.GeneralSlider].AllocateGameObject<GeneralSlider>(WorldUIManager.Instance.Canvas.transform);
        GeneralSlider.FollowObject = transform;
    }
}
