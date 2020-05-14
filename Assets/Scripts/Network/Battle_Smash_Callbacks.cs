[BoltGlobalBehaviour("Battle_Smash")]
public class Battle_Smash_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(BattleStartEvent evnt)
    {
        GameManager.Instance.Cur_BallBattleManager.StartBattle();
    }

    public override void OnEvent(PlayerRingEvent evnt)
    {
        Player player = GameManager.Instance.Cur_BattleManager.GetPlayer((PlayerNumber) evnt.PlayerNumber);
        if (evnt.HasRing)
        {
            player.GetRing((CostumeType) evnt.CostumeType);
        }
        else
        {
            player.LoseRing(evnt.Exploded);
        }
    }

    public override void OnEvent(ScoreChangeEvent evnt)
    {
        TeamNumber tn = (TeamNumber) evnt.TeamNumber;
        Team team = GameManager.Instance.Cur_BattleManager.TeamDict[tn];
        team.Score = evnt.Score;
        team.MegaScore = evnt.MegaScore;
        if (!evnt.IsNewBattle)
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
        if (GameManager.Instance.Cur_BattleManager && GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
        {
            smash.StartNewRound(evnt.Round);
        }
    }

    public override void OnEvent(RoundEndEvent evnt)
    {
        if (GameManager.Instance.Cur_BattleManager && GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
        {
            smash.EndRound((TeamNumber) evnt.WinTeamNumber, evnt.Team1Score, evnt.Team2Score);
        }
    }
}