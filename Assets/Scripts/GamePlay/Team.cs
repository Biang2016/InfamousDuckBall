using System.Collections.Generic;

public class Team
{
    public TeamNumber TeamNumber;
    public int Score;
    public int MegaScore;

    public List<Player> TeamPlayers = new List<Player>();

    public Team(TeamNumber teamNumber)
    {
        TeamNumber = teamNumber;
    }
}

public enum TeamNumber
{
    Team1 = 0, // Red Team
    Team2 = 1, // Blue Team
    Team3 = 2,
    Team4 = 3,
    AnyTeam = 99,
    None = -1,
}