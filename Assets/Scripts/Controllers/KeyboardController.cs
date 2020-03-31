using UnityEngine;

public class KeyBoardController : Controller
{
    public const float KeyboardThreshold = 0.8f;

    public override void FixedUpdate()
    {
        ButtonPressed[ControlButtons.StartButton] = Input.GetButtonDown("Start_" + ControllerIndex);

        ButtonPressed[ControlButtons.LeftTrigger] = Input.GetMouseButton(0);
        ButtonPressed[ControlButtons.RightTrigger] = Input.GetMouseButton(1);

        float leftHorizontal = Input.GetAxis("LH_" + ControllerIndex);
        float leftVertical = Input.GetAxis("LV_" + ControllerIndex);

        Axises[ControlAxis.LeftStick_H] = Mathf.Abs(leftHorizontal) > KeyboardThreshold ? leftHorizontal : 0;
        Axises[ControlAxis.LeftStick_V] = Mathf.Abs(leftVertical) > KeyboardThreshold ? leftVertical : 0;

        ButtonPressed[ControlButtons.LeftStickRight] = leftHorizontal > KeyboardThreshold;
        ButtonPressed[ControlButtons.LeftStickLeft] = leftHorizontal < -KeyboardThreshold;

        ButtonPressed[ControlButtons.LeftStickUp] = leftVertical < -KeyboardThreshold;
        ButtonPressed[ControlButtons.LeftStickDown] = leftVertical > KeyboardThreshold;

        base.FixedUpdate();
    }
}