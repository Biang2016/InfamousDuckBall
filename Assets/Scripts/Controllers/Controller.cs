using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    public PlayerNumber ControllerNumber = PlayerNumber.None;
    public int ControllerIndex = 0;
    public Dictionary<ControlAxis, float> Axises = new Dictionary<ControlAxis, float>();
    public Dictionary<ControlButtons, bool> ButtonPressedLastFrame = new Dictionary<ControlButtons, bool>();
    public Dictionary<ControlButtons, bool> ButtonPressed = new Dictionary<ControlButtons, bool>();
    public Dictionary<ControlButtons, bool> ButtonDown = new Dictionary<ControlButtons, bool>();
    public Dictionary<ControlButtons, bool> ButtonUp = new Dictionary<ControlButtons, bool>();

    public void Init(PlayerNumber controllerNumber)
    {
        ControllerIndex = (int) controllerNumber + 1;
        ControllerNumber = controllerNumber;
        foreach (object b in Enum.GetValues(typeof(ControlButtons)))
        {
            ControlButtons btn = (ControlButtons) b;
            ButtonPressedLastFrame.Add(btn, false);
            ButtonPressed.Add(btn, false);
            ButtonDown.Add(btn, false);
            ButtonUp.Add(btn, false);
        }

        foreach (object b in Enum.GetValues(typeof(ControlAxis)))
        {
            ControlAxis ca = (ControlAxis) b;
            Axises.Add(ca, 0);
        }
    }

    public virtual void FixedUpdate()
    {
        foreach (KeyValuePair<ControlButtons, bool> kv in ButtonPressed)
        {
            ButtonDown[kv.Key] = ButtonPressed[kv.Key] && !ButtonPressedLastFrame[kv.Key];
            ButtonUp[kv.Key] = !ButtonPressed[kv.Key] && ButtonPressedLastFrame[kv.Key];

            //if (ButtonUp[kv.Key]) Debug.Log(kv.Key);
        }

        foreach (KeyValuePair<ControlButtons, bool> kv in ButtonPressed)
        {
            ButtonPressedLastFrame[kv.Key] = kv.Value;
        }
    }

    public bool AnyButtonPressed()
    {
        foreach (KeyValuePair<ControlButtons, bool> kv in ButtonPressed)
        {
            if (kv.Value) return true;
        }

        return false;
    }
}

public enum ControlButtons
{
    LeftStickUp,
    LeftStickDown,
    LeftStickLeft,
    LeftStickRight,
    RightStickUp,
    RightStickDown,
    RightStickLeft,
    RightStickRight,
    LeftBumper,
    RightBumper,
    LeftTrigger,
    RightTrigger,
    StartButton,
    DPAD_Up,
    DPAD_Down,
    DPAD_Left,
    DPAD_Right,
    A,
    B,
    X,
    Y,
}

public enum ControlAxis
{
    LeftStick_H,
    LeftStick_V,
    RightStick_H,
    RightStick_V,
}