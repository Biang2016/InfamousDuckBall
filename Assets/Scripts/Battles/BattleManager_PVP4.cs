public class BattleManager_PVP4 : BattleManager
{
    protected override void Child_Initialize()
    {
        base.Child_Initialize();
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player1, TeamNumber.Team1));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player2, TeamNumber.Team2));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player3, TeamNumber.Team3));
        GameManager.Instance.SetUpPlayer(new PlayerInfo(PlayerNumber.Player4, TeamNumber.Team4));
    }
}