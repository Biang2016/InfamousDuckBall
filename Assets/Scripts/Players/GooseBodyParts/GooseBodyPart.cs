using UnityEngine;
using System.Collections;

public abstract class GooseBodyPart : Controllable
{
    internal Goose Goose;

    protected virtual void Awake()
    {
        Goose = GetComponentInParent<Goose>();
    }
}