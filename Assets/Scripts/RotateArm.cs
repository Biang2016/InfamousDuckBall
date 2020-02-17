using UnityEngine;

public class RotateArm : MonoBehaviour
{
    //[SerializeField] private TextMeshPro KeyLabel;
    [SerializeField] private Vector3 RotateDir;
    [SerializeField] private string RotateKeyStr;
    [SerializeField] private JoystickAxis Axis = JoystickAxis.None;

    private float CurOffset;
    [SerializeField] private float UpperLimit = 10f;
    [SerializeField] private float LowerLimit = -10f;

    void Awake()
    {
        //if (Axis != JoystickAxis.None)
        //{
        //    KeyLabel.text = Axis.ToString();
        //}
        //else
        //{
        //    KeyLabel.text = "";
        //}

        CurOffset = 0;
    }

    void Update()
    {
        if (Axis != JoystickAxis.None)
        {
            if (Input.GetAxis(Axis.ToString()) < -0.8f)
            {
                if (CurOffset > LowerLimit)
                {
                    transform.Rotate(-RotateDir);
                    CurOffset -= RotateDir.magnitude;
                }
            }
            else if (Input.GetAxis(Axis.ToString()) > 0.8f)
            {
                if (CurOffset < UpperLimit)
                {
                    transform.Rotate(RotateDir);
                    CurOffset += RotateDir.magnitude;
                }
            }
        }
    }

    void LateUpdate()
    {
        //Vector3 targetPos = KeyLabel.transform.position + Camera.main.transform.rotation * Vector3.forward;
        //Vector3 targetOrientation = Camera.main.transform.rotation * Vector3.up;
        //KeyLabel.transform.LookAt(targetPos, targetOrientation);
    }
}

