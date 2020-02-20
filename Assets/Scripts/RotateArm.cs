using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public PlayerNumber PlayerNumber;

    [SerializeField] private Vector3 RotateDir;
    [SerializeField] private string RotateKeyStr;
    [SerializeField] private JoystickAxis Axis = JoystickAxis.None;

    private float CurOffset;
    [SerializeField] private float UpperLimit = 10f;
    [SerializeField] private float LowerLimit = -10f;

    void Awake()
    {
        CurOffset = 0;
    }

    void Update()
    {
        if (Axis != JoystickAxis.None)
        {
            if (Input.GetAxis(Axis + "_" + PlayerNumber).Equals(-1))
            {
                if (CurOffset > LowerLimit)
                {
                    transform.Rotate(-RotateDir);
                    CurOffset -= RotateDir.magnitude;
                }
            }
            else if (Input.GetAxis(Axis + "_" + PlayerNumber).Equals(1))
            {
                if (CurOffset < UpperLimit)
                {
                    transform.Rotate(RotateDir);
                    CurOffset += RotateDir.magnitude;
                }
            }
        }
    }
}