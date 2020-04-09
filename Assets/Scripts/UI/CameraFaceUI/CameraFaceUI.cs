﻿using UnityEngine;
using System.Collections;

public abstract class CameraFaceUI : PoolObject
{
    public Transform FollowObject;

    void LateUpdate()
    {
        if (!IsRecycled)
        {
            if (FollowObject == null)
            {
                PoolRecycle();
            }
            else
            {
                Vector3 objectScreenPosition = GameManager.Cur_BattleManager.BattleCamera.WorldToScreenPoint(FollowObject.position);
                ((RectTransform) transform).position = objectScreenPosition;
            }
        }
    }
}