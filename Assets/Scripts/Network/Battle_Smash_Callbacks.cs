using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            player.LoseRing();
        }
    }

    public override void OnEvent(ScoreChangeEvent evnt)
    {
        TeamNumber tn = (TeamNumber) evnt.TeamNumber;
        Team team = GameManager.Instance.Cur_BattleManager.TeamDict[tn];
        team.Score = evnt.Score;
        team.MegaScore = evnt.MegaScore;
        AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        GameManager.Instance.DebugPanel.RefreshScore(true);
    }
}