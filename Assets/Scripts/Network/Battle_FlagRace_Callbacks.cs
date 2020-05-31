[BoltGlobalBehaviour("Battle_FlagRace")]
public class Battle_FlagRace_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(ScoreChangeEvent evnt)
    {
        OnEvent_ScoreChangeEvent(evnt.TeamNumber, evnt.Score, evnt.IsNewBattle, evnt.ScorePlayer);
    }

    public static void OnEvent_ScoreChangeEvent(int teamNumber, int score, bool isNewBattle, int scorePlayer)
    {
        TeamNumber tn = (TeamNumber) teamNumber;
        Team team = GameManager.Instance.Cur_BattleManager.TeamDict[tn];
        team.Score = score;
        if (!isNewBattle)
        {
            if (tn == TeamNumber.Team1)
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().RefreshScore_Team1(team.Score);
            }
            else if (tn == TeamNumber.Team2)
            {
                UIManager.Instance.GetBaseUIForm<RoundSmallScorePanel>().RefreshScore_Team2(team.Score);
            }

            PlayerNumber playerNumber = (PlayerNumber) scorePlayer;
            AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyInPlace, GameManager.Instance.Cur_BattleManager.GetPlayer(playerNumber).Duck.gameObject);
        }
    }

    public override void OnEvent(PlayerTeamChangeEvent evnt)
    {
        OnEvent_PlayerTeamChangeEvent(evnt.TeamNumber, evnt.OriTeamNumber, evnt.PlayerNumber);
    }

    public static void OnEvent_PlayerTeamChangeEvent(int teamNumber, int oriTeamNumber, int playerNumber)
    {
        TeamNumber newTeamNumber = (TeamNumber) teamNumber;
        TeamNumber oldTeamNumber = (TeamNumber) oriTeamNumber;
        PlayerNumber pn = (PlayerNumber) playerNumber;
        Player player = GameManager.Instance.Cur_BattleManager.GetPlayer(pn);
        GameManager.Instance.Cur_BattleManager.TeamDict[oldTeamNumber].TeamPlayers.Remove(player);
        GameManager.Instance.Cur_BattleManager.TeamDict[newTeamNumber].TeamPlayers.Add(player);
        player.PlayerCostume.Initialize(pn, newTeamNumber, player.CostumeType);
    }
}