using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Spring : MonoBehaviour
{
    public PlayerNumber PlayerNumber;

    [SerializeField] private JoystickButton Button = JoystickButton.None;
    [SerializeField] private Bumper Bumper;

    private Vector3 DefaultPos;
    [SerializeField] private Vector3 BumperDest;

    void Awake()
    {
        DefaultPos = Bumper.transform.localPosition;
    }

    void FixedUpdate()
    {
        if (Button != JoystickButton.None)
        {
            if (Input.GetButtonDown(Button + "_" + PlayerNumber))
            {
                Bumper.Kick();
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.Ball.transform);
    }
}