public class PlayerInfo
{
    private static int IndexGenerator = 1000;

    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
    public int RobotIndex = -1;

    public PlayerInfo(PlayerNumber playerNumber, TeamNumber teamNumber)
    {
        PlayerNumber = playerNumber;
        TeamNumber = teamNumber;
        RobotIndex = IndexGenerator;
        IndexGenerator++;
    }

    public PlayerInfo Clone()
    {
        return new PlayerInfo(PlayerNumber, TeamNumber);
    }
}