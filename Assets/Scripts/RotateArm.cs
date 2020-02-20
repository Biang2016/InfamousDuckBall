using UnityEngine;

public class RotateArm : MonoBehaviour
{
    [SerializeField] private Vector3 RotateDir;
    [SerializeField] private string RotateKeyStr;

    [SerializeField] private bool UseLimit = true;
    [SerializeField] private float UpperLimit = 10f;
    [SerializeField] private float LowerLimit = -10f;

    [SerializeField] private Transform Model;

    internal float Length
    {
        get
        {
            if (Model)
            {
                return Model.transform.localScale.y * 2;
            }

            return 0;
        }
    }

    void Awake()
    {
    }

    public void SetRotation(float angle)
    {
        if (UseLimit)
        {
            angle = Mathf.Clamp(angle, UpperLimit, LowerLimit);
        }

        transform.localRotation = Quaternion.Euler(RotateDir * angle);
    }
}