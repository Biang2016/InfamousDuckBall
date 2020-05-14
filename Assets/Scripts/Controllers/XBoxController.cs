using UnityEngine;

public class XBoxController : Controller
{
    public const float JoyStickThreshold = 0.8f;

    public override void FixedUpdate()
    {
        ButtonPressed[ControlButtons.StartButton] = Active && Input.GetButtonDown("Start_" + ControllerIndex);
        ButtonPressed[ControlButtons.A] = Active && Input.GetButtonDown("A_" + ControllerIndex);
        ButtonPressed[ControlButtons.B] = Active && Input.GetButtonDown("B_" + ControllerIndex);
        ButtonPressed[ControlButtons.X] = Active && Input.GetButtonDown("X_" + ControllerIndex);
        ButtonPressed[ControlButtons.Y] = Active && Input.GetButtonDown("Y_" + ControllerIndex);
        ButtonPressed[ControlButtons.LeftBumper] = Active && Input.GetButtonDown("LB_" + ControllerIndex);
        ButtonPressed[ControlButtons.RightBumper] = Active && Input.GetButtonDown("RB_" + ControllerIndex);

        float leftTrigger = Active ? (Input.GetAxis("LT_" + ControllerIndex)) : 0;
        float rightTrigger = Active ? (Input.GetAxis("RT_" + ControllerIndex)) : 0;

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // leftTrigger = (leftTrigger + 1) / 2f;
        // rightTrigger = (rightTrigger + 1) / 2f;
#endif
        ButtonPressed[ControlButtons.LeftTrigger] = Active && (leftTrigger > JoyStickThreshold || leftTrigger < -JoyStickThreshold);
        ButtonPressed[ControlButtons.RightTrigger] = Active && (rightTrigger > JoyStickThreshold || rightTrigger < -JoyStickThreshold);

        float leftHorizontal = Active ? Input.GetAxis("LH_" + ControllerIndex) : 0;
        float rightHorizontal = Active ? Input.GetAxis("RH_" + ControllerIndex) : 0;
        float leftVertical = Active ? Input.GetAxis("LV_" + ControllerIndex) : 0;
        float rightVertical = Active ? Input.GetAxis("RV_" + ControllerIndex) : 0;

        Axises[ControlAxis.LeftStick_H] = Active ? (Mathf.Abs(leftHorizontal) > JoyStickThreshold ? leftHorizontal : 0) : 0;
        Axises[ControlAxis.RightStick_H] = Active ? (Mathf.Abs(rightHorizontal) > JoyStickThreshold ? rightHorizontal : 0) : 0;
        Axises[ControlAxis.LeftStick_V] = Active ? (Mathf.Abs(leftVertical) > JoyStickThreshold ? leftVertical : 0) : 0;
        Axises[ControlAxis.RightStick_V] = Active ? (Mathf.Abs(rightVertical) > JoyStickThreshold ? rightVertical : 0) : 0;

        ButtonPressed[ControlButtons.LeftStickRight] = Active && leftHorizontal > JoyStickThreshold;
        ButtonPressed[ControlButtons.RightStickRight] = Active && rightHorizontal > JoyStickThreshold;
        ButtonPressed[ControlButtons.LeftStickLeft] = Active && leftHorizontal < -JoyStickThreshold;
        ButtonPressed[ControlButtons.RightStickLeft] = Active && rightHorizontal < -JoyStickThreshold;

        ButtonPressed[ControlButtons.LeftStickUp] = Active && leftVertical < -JoyStickThreshold;
        ButtonPressed[ControlButtons.RightStickUp] = Active && rightVertical < -JoyStickThreshold;
        ButtonPressed[ControlButtons.LeftStickDown] = Active && leftVertical > JoyStickThreshold;
        ButtonPressed[ControlButtons.RightStickDown] = Active && rightVertical > JoyStickThreshold;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        float dpad_h = Active ? (Input.GetAxis("DPAD_H_" + ControllerIndex)) : 0;
        float dpad_v = Active ? (Input.GetAxis("DPAD_V_" + ControllerIndex)) : 0;

        ButtonPressed[ControlButtons.DPAD_Up] = Active && dpad_v > JoyStickThreshold;
        ButtonPressed[ControlButtons.DPAD_Down] = Active && dpad_v < -JoyStickThreshold;
        ButtonPressed[ControlButtons.DPAD_Left] = Active && dpad_h < -JoyStickThreshold;
        ButtonPressed[ControlButtons.DPAD_Right] = Active && dpad_h > JoyStickThreshold;
#endif
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        ButtonPressed[ControlButtons.DPAD_Up] = Active && Input.GetButton("DPAD_Up_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Down] = Active && Input.GetButton("DPAD_Down_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Left] = Active && Input.GetButton("DPAD_Left_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Right] = Active && Input.GetButton("DPAD_Right_" + ControllerIndex);
#endif

        base.FixedUpdate();
    }
}