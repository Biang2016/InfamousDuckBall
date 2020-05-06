public class PlayerInfoData
{
    public string PlayerName;
    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
    public CostumeType CostumeType;

    public PlayerInfoData(string playerName, PlayerNumber playerNumber, TeamNumber teamNumber, CostumeType costumeType)
    {
        PlayerName = playerName;
        PlayerNumber = playerNumber;
        TeamNumber = teamNumber;
        CostumeType = costumeType;
    }

    public PlayerInfoData Clone()
    {
        return new PlayerInfoData(PlayerName, PlayerNumber, TeamNumber, CostumeType);
    }
}