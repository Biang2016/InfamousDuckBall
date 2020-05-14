using UnityEngine;

public class RingContainer : MonoBehaviour
{
    [SerializeField] private Transform Pivot;

    private Quaternion InitRotation;

    void Awake()
    {
        InitRotation = Pivot.rotation;
    }

    void LateUpdate()
    {
        Vector3 pos = Pivot.position;
        transform.position = pos;

        Quaternion rot = Pivot.rotation * Quaternion.Inverse(InitRotation);
        transform.rotation = rot;
    }
}