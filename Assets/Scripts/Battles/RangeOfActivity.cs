using UnityEngine;
using System.Collections;

public class RangeOfActivity : MonoBehaviour
{
    public RangeType M_RangeType;
    private Collider Collider;

    void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    public enum RangeType
    {
        Allowed = 0,
        Forbidden = 1
    }
}