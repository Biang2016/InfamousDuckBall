[BoltGlobalBehaviour("Battle_Smash")]
public class Battle_Smash_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(ScoreChangeEvent evnt)
    {
        OnEvent_ScoreChangeEvent(evnt.TeamNumber, evnt.Score, evnt.MegaScore, evnt.IsNewBattle);
    }

    public static void OnEvent_ScoreChangeEvent(int teamNumber, int score, int megaScore, bool isNewBattle)
    {
        TeamNumber tn = (TeamNumber) teamNumber;
        Team team = GameManager.Instance.Cur_BattleManager.TeamDict[tn];
        team.Score = score;
        team.MegaScore = megaScore;
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

            AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        }
    }

    public override void OnEvent(RoundStartEvent evnt)
    {
        OnEvent_RoundStartEvent(evnt.Round);
    }

    public static void OnEvent_RoundStartEvent(int round)
    {
        if (GameManager.Instance.Cur_BattleManager && GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
        {
            smash.StartNewRound(round);
        }
    }

    public override void OnEvent(RoundEndEvent evnt)
    {
        OnEvent_RoundEndEvent(evnt.WinTeamNumber, evnt.Team1Score, evnt.Team2Score);
    }

    public static void OnEvent_RoundEndEvent(int winTeamNumber, int team1Score, int team2Score)
    {
        if (GameManager.Instance.Cur_BattleManager && GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
        {
            smash.EndRound((TeamNumber) winTeamNumber, team1Score, team2Score);
        }
    }
}