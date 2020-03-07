public class BattleManager_PVP4 : BattleManager
{
    protected override void Child_Initialize()
    {
        base.Child_Initialize();
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player1, PlayerType.ArmSpringHammer));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player2, PlayerType.ArmSpringHammer));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player3, PlayerType.ArmSpringHammer));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player4, PlayerType.ArmSpringHammer));
    }
}