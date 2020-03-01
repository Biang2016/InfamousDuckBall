using UnityEngine;
using System.Collections;

public abstract class PlayerUpperPart : Controllable
{
    internal Transform PlayerBase;

    protected virtual void LateUpdate()
    {
        transform.position = PlayerBase.position;
    }
}