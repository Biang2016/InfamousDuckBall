﻿public class BattleManager_PVP2 : BattleManager
{
    protected override void Child_Initialize()
    {
        base.Child_Initialize();
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player1));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player2));
    }
}