using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            player.LoseRing();
        }
    }

    public override void OnEvent(ScoreChangeEvent evnt)
    {
        TeamNumber tn = (TeamNumber) evnt.TeamNumber;
        Team team = GameManager.Instance.Cur_BattleManager.TeamDict[tn];
        team.Score = evnt.Score;
        if (!evnt.IsNewBattle)
        {
            AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        }
        GameManager.Instance.DebugPanel.RefreshScore(false);
    }

    IEnumerator Co_ScoreRingDisappear(TeamNumber tn, int score)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
    }
}