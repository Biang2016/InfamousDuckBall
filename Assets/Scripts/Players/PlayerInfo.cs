public class PlayerInfo
{
    private static int IndexGenerator = 1000;

    public PlayerNumber PlayerNumber;
    public int RobotIndex = -1;


    public PlayerInfo(PlayerNumber playerNumber)
    {
        PlayerNumber = playerNumber;
        RobotIndex = IndexGenerator;
        IndexGenerator++;
    }

    public PlayerInfo Clone()
    {
        return new PlayerInfo(PlayerNumber);
    }
}