public class BattleManager_PVP4 : BattleManager
{
    protected override void Child_Initialize()
    {
        base.Child_Initialize();
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player1));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player2));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player3));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player4));
    }
}