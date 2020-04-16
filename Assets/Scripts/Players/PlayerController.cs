using System;
using Bolt;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player Player;
    public Controller Controller;

    private float leftHorizontal;
    private float leftVertical;
    private Vector3 headTargetPos;
    private Vector3 headLookAtPos;

    bool leftTriggerDown = false;

    private bool rightBumperDown = false;
    bool rightTriggerDown = false;
    bool rightTriggerUp = false;
    bool rightTriggerPressed = false;

    bool dpad_LeftUp = false;
    bool dpad_RightUp = false;

    public void Attached()
    {
    }

    void PollKeys(bool mouse)
    {
        if (Player.Controller != null)
        {
            leftHorizontal = Player.Controller.Axises[ControlAxis.LeftStick_H];
            leftVertical = Player.Controller.Axises[ControlAxis.LeftStick_V];
            headTargetPos = Player.Duck.Body.Cur_HeadTargetPosition;
            headLookAtPos = Player.Duck.Head.Cur_HeadLookAtPosition;

            leftTriggerDown = Player.Controller.ButtonDown[ControlButtons.LeftTrigger];

            rightBumperDown = Player.Controller.ButtonDown[ControlButtons.RightBumper];
            rightTriggerDown = Player.Controller.ButtonDown[ControlButtons.RightTrigger];
            rightTriggerUp = Player.Controller.ButtonUp[ControlButtons.RightTrigger];
            rightTriggerPressed = Player.Controller.ButtonPressed[ControlButtons.RightTrigger];

            dpad_LeftUp = Player.Controller.ButtonUp[ControlButtons.DPAD_Left];
            dpad_RightUp = Player.Controller.ButtonUp[ControlButtons.DPAD_Right];
        }
    }

    void Update()
    {
        PollKeys(true);
    }

    public void SimulateController()
    {
        PollKeys(false);

        IPlayerCommandInput input = PlayerCommand.Create();

        input.LeftHorizontal = leftHorizontal;
        input.LeftVertical = leftVertical;
        input.HeadTargetPos = headTargetPos;
        input.HeadLookAtPos = headLookAtPos;

        input.LeftTriggerDown = leftTriggerDown;
        input.RightBumperDown = rightBumperDown;
        input.RightTriggerDown = rightTriggerDown;
        input.RightTriggerUp = rightTriggerUp;
        input.RightTrigger = rightTriggerPressed;

        input.DPAD_LeftUp = dpad_LeftUp;
        input.DPAD_RightUp = dpad_RightUp;

        Player.entity.QueueInput(input);
    }

    public void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand) command;

        if (resetState)
        {
            // we got a correction from the server, reset (this only runs on the client)
            Player.Duck.Feet.transform.position = cmd.Result.FeetPosition;
            Player.Duck.DuckRigidbody.velocity = cmd.Result.PlayerVelocity;
            Player.Duck.DuckRigidbody.angularVelocity = cmd.Result.PlayerAngularVelocity;
        }
        else
        {
            // apply movement (this runs on both server and client)
            Vector3 diff = Vector3.zero;
            diff += Vector3.forward * cmd.Input.LeftHorizontal;
            diff += Vector3.right * cmd.Input.LeftVertical;
            diff = Vector3.ClampMagnitude(diff, 1) * (Player.DuckConfig.Accelerate * Player.DuckConfig.MoveSpeedModifier);

            Player.Duck.DuckRigidbody.AddForce(diff);
            Player.Duck.Head.ExecuteCommand(cmd.Input.LeftTriggerDown, cmd.Input.RightBumperDown, cmd.Input.RightTriggerDown, cmd.Input.RightTriggerUp, cmd.Input.RightTrigger);

            // copy the motor state to the commands result (this gets sent back to the client)
            cmd.Result.PlayerVelocity = Player.Duck.DuckRigidbody.velocity;
            cmd.Result.PlayerAngularVelocity = Player.Duck.DuckRigidbody.angularVelocity;

            cmd.Result.FeetPosition = Player.GetPlayerPosition;
            Player.state.FeetPosition = cmd.Result.FeetPosition;

            if (Player.entity.IsOwner)
            {
                if (!GameManager.Cur_BattleManager.IsStart)
                {
                    if (cmd.Input.DPAD_RightUp || cmd.Input.DPAD_LeftUp)
                    {
                        TeamNumber oldTeamNumber = Player.TeamNumber;
                        TeamNumber newTeamNumber = Player.TeamNumber == TeamNumber.Team1 ? TeamNumber.Team2 : TeamNumber.Team1;
                        Player.state.PlayerInfo.TeamNumber = (int) newTeamNumber;
                        PlayerTeamChangeEvent evnt = PlayerTeamChangeEvent.Create();
                        evnt.PlayerNumber = (int) Player.PlayerNumber;
                        evnt.TeamNumber = (int) newTeamNumber;
                        evnt.OriTeamNumber = (int) oldTeamNumber;
                        evnt.Send();
                    }
                }

                Player.state.HeadTargetPosition = cmd.Input.HeadTargetPos;
                Player.state.HeadLookAtPosition = cmd.Input.HeadLookAtPos;

                Player.state.Input.LeftTriggerDown = cmd.Input.LeftTriggerDown;
                Player.state.Input.RightBumperDown = cmd.Input.RightBumperDown;
                Player.state.Input.RightTriggerDown = cmd.Input.RightTriggerDown;
                Player.state.Input.RightTriggerUp = cmd.Input.RightTriggerUp;
                Player.state.Input.RightTrigger = cmd.Input.RightTrigger;
            }
        }
    }

    private void LateUpdate()
    {
        if (!Player.entity.IsControllerOrOwner)
        {
            Player.Duck.Feet.transform.position = Player.state.FeetPosition;
        }
    }
}