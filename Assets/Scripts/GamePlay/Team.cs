using System.Collections.Generic;
using Bolt;

public class Team
{
    public TeamNumber TeamNumber;
    public int Score;

    public List<Player> TeamPlayers = new List<Player>();

    public Team(TeamNumber teamNumber, int score)
    {
        TeamNumber = teamNumber;
        Score = score;
    }
}

public enum TeamNumber
{
    Team1 = 0, // Red Team
    Team2 = 1, // Blue Team
    Team3 = 2,
    Team4 = 3,
}