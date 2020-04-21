public class PlayerInfoData
{
    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
    public CostumeType CostumeType;

    public PlayerInfoData(PlayerNumber playerNumber, TeamNumber teamNumber, CostumeType costumeType)
    {
        PlayerNumber = playerNumber;
        TeamNumber = teamNumber;
        CostumeType = costumeType;
    }

    public PlayerInfoData Clone()
    {
        return new PlayerInfoData(PlayerNumber, TeamNumber, CostumeType);
    }
}