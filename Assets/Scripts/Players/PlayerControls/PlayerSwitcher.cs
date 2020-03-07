using System;
using UnityEngine;
using System.Collections;

public class PlayerSwitcher : Controllable
{
    private int PlayerTypesCount = 0;

    void Awake()
    {
        PlayerTypesCount = Enum.GetNames(typeof(PlayerType)).Length;
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.DPAD_Left])
        {
            PlayerType newPlayerType = (PlayerType) (((int) (ParentPlayerControl.Player.PlayerInfo.PlayerType) + 1) % PlayerTypesCount);
            PlayerInfo newPlayerInfo = new PlayerInfo(ParentPlayerControl.Player.PlayerInfo.PlayerNumber, newPlayerType);
            GameManager.Instance.ReplacePlayer(ParentPlayerControl.Player, newPlayerInfo);
        }
    }

    protected override void Operate_AI()
    {
    }
}