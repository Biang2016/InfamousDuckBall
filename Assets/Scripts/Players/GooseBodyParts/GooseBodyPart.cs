using UnityEngine;
using System.Collections;

public abstract class GooseBodyPart : Controllable
{
    internal Goose Goose;
    internal GooseConfig GooseConfig;

    protected virtual void Awake()
    {
        Goose = GetComponentInParent<Goose>();
        GooseConfig = GetComponentInParent<GooseConfig>();
    }
}