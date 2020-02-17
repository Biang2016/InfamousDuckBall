using System;
using UnityEngine;

public class RotateArm : MonoBehaviour
{
    public Vector3 RotateDir;
    public string RotateKeyStr;
    private KeyCode RotateKey;

    void Awake()
    {
        RotateKey = (KeyCode) Enum.Parse(typeof(KeyCode), RotateKeyStr, true);
    }

    void Update()
    {
        if (Input.GetKey(RotateKey))
        {
            transform.Rotate(RotateDir);
        }
    }
}