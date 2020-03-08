using UnityEngine;
using System.Collections;

public abstract class GooseBodyPart : Controllable
{
    internal Goose Goose;

    void Awake()
    {
        Goose = GetComponentInParent<Goose>();
    }
}