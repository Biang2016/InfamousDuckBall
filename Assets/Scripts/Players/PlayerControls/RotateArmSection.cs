using UnityEngine;

public class RotateArmSection : MonoBehaviour
{
    [SerializeField] private Vector3 RotateDir;

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
        transform.localRotation = Quaternion.Euler(RotateDir * angle);
    }
}