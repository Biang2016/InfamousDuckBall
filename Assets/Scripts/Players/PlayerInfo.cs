public class PlayerInfo
{
    private static int IndexGenerator = 1000;

    public PlayerNumber PlayerNumber;
    public PlayerType PlayerType;
    public int RobotIndex = -1;


    public PlayerInfo(PlayerNumber playerNumber, PlayerType playerType)
    {
        PlayerNumber = playerNumber;
        PlayerType = playerType;
        RobotIndex = IndexGenerator;
        IndexGenerator++;
    }

    public PlayerInfo Clone()
    {
        return new PlayerInfo(PlayerNumber, PlayerType);
    }
}