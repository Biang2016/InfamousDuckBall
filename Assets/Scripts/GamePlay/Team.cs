using System.Collections.Generic;

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
    Team1 = 0,
    Team2 = 1,
    Team3 = 2,
    Team4 = 3,
}