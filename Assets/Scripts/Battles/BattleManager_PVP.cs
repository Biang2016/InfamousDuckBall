public class BattleManager_PVP : BattleManager
{
    protected override void Child_Initialize()
    {
        base.Child_Initialize();
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player1, PlayerType.ArmSpringHammer));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player2, PlayerType.ArmSpringHammer));
    }
}