using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{

    private void OnTriggerEnter(Collider c)
    {
        RotateArm RotateArm = c.gameObject.GetComponentInParent<RotateArm>();
        if (RotateArm)
        {
            transform.localScale *= 2;
        } 
    }
}
