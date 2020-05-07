[BoltGlobalBehaviour("Battle_FlagRace")]
public class Battle_FlagRace_Callbacks : Bolt.GlobalEventListener
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

            PlayerNumber playerNumber = (PlayerNumber) evnt.ScorePlayer;
            AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyInPlace, GameManager.Instance.Cur_BattleManager.GetPlayer(playerNumber).gameObject);
        }
    }

    public override void OnEvent(PlayerTeamChangeEvent evnt)
    {
        TeamNumber newTeamNumber = (TeamNumber) evnt.TeamNumber;
        TeamNumber oldTeamNumber = (TeamNumber) evnt.OriTeamNumber;
        PlayerNumber pn = (PlayerNumber) evnt.PlayerNumber;
        Player player = GameManager.Instance.Cur_BattleManager.GetPlayer(pn);
        GameManager.Instance.Cur_BattleManager.TeamDict[oldTeamNumber].TeamPlayers.Remove(player);
        GameManager.Instance.Cur_BattleManager.TeamDict[newTeamNumber].TeamPlayers.Add(player);
        player.PlayerCostume.Initialize(pn, newTeamNumber, player.CostumeType);
    }
}