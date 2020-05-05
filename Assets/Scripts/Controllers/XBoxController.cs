using UnityEngine;

public class XBoxController : Controller
{
    public const float JoyStickThreshold = 0.8f;

    public override void FixedUpdate()
    {
        if (Active)
        {
            ButtonPressed[ControlButtons.StartButton] = Input.GetButtonDown("Start_" + ControllerIndex);
            ButtonPressed[ControlButtons.A] = Input.GetButtonDown("A_" + ControllerIndex);
            ButtonPressed[ControlButtons.B] = Input.GetButtonDown("B_" + ControllerIndex);
            ButtonPressed[ControlButtons.X] = Input.GetButtonDown("X_" + ControllerIndex);
            ButtonPressed[ControlButtons.Y] = Input.GetButtonDown("Y_" + ControllerIndex);
            ButtonPressed[ControlButtons.LeftBumper] = Input.GetButtonDown("LB_" + ControllerIndex);
            ButtonPressed[ControlButtons.RightBumper] = Input.GetButtonDown("RB_" + ControllerIndex);

            float leftTrigger = Input.GetAxis("LT_" + ControllerIndex);
            float rightTrigger = Input.GetAxis("RT_" + ControllerIndex);

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // leftTrigger = (leftTrigger + 1) / 2f;
        // rightTrigger = (rightTrigger + 1) / 2f;
#endif
            ButtonPressed[ControlButtons.LeftTrigger] = leftTrigger > JoyStickThreshold || leftTrigger < -JoyStickThreshold;
            ButtonPressed[ControlButtons.RightTrigger] = rightTrigger > JoyStickThreshold || rightTrigger < -JoyStickThreshold;

            float leftHorizontal = Input.GetAxis("LH_" + ControllerIndex);
            float rightHorizontal = Input.GetAxis("RH_" + ControllerIndex);
            float leftVertical = Input.GetAxis("LV_" + ControllerIndex);
            float rightVertical = Input.GetAxis("RV_" + ControllerIndex);

            Axises[ControlAxis.LeftStick_H] = Mathf.Abs(leftHorizontal) > JoyStickThreshold ? leftHorizontal : 0;
            Axises[ControlAxis.RightStick_H] = Mathf.Abs(rightHorizontal) > JoyStickThreshold ? rightHorizontal : 0;
            Axises[ControlAxis.LeftStick_V] = Mathf.Abs(leftVertical) > JoyStickThreshold ? leftVertical : 0;
            Axises[ControlAxis.RightStick_V] = Mathf.Abs(rightVertical) > JoyStickThreshold ? rightVertical : 0;

            ButtonPressed[ControlButtons.LeftStickRight] = leftHorizontal > JoyStickThreshold;
            ButtonPressed[ControlButtons.RightStickRight] = rightHorizontal > JoyStickThreshold;
            ButtonPressed[ControlButtons.LeftStickLeft] = leftHorizontal < -JoyStickThreshold;
            ButtonPressed[ControlButtons.RightStickLeft] = rightHorizontal < -JoyStickThreshold;

            ButtonPressed[ControlButtons.LeftStickUp] = leftVertical < -JoyStickThreshold;
            ButtonPressed[ControlButtons.RightStickUp] = rightVertical < -JoyStickThreshold;
            ButtonPressed[ControlButtons.LeftStickDown] = leftVertical > JoyStickThreshold;
            ButtonPressed[ControlButtons.RightStickDown] = rightVertical > JoyStickThreshold;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            float dpad_h = Input.GetAxis("DPAD_H_" + ControllerIndex);
            float dpad_v = Input.GetAxis("DPAD_V_" + ControllerIndex);

            ButtonPressed[ControlButtons.DPAD_Up] = dpad_v > JoyStickThreshold;
            ButtonPressed[ControlButtons.DPAD_Down] = dpad_v < -JoyStickThreshold;
            ButtonPressed[ControlButtons.DPAD_Left] = dpad_h < -JoyStickThreshold;
            ButtonPressed[ControlButtons.DPAD_Right] = dpad_h > JoyStickThreshold;
#endif
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        ButtonPressed[ControlButtons.DPAD_Up] = Input.GetButton("DPAD_Up_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Down] = Input.GetButton("DPAD_Down_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Left] = Input.GetButton("DPAD_Left_" + ControllerIndex);
        ButtonPressed[ControlButtons.DPAD_Right] = Input.GetButton("DPAD_Right_" + ControllerIndex);
#endif
        }

        base.FixedUpdate();
    }
}