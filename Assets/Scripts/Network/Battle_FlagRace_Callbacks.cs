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
            PlayerNumber playerNumber = (PlayerNumber) evnt.ScorePlayer;
            AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyInPlace, GameManager.Instance.Cur_BattleManager.GetPlayer(playerNumber).gameObject);
        }

        GameManager.Instance.DebugPanel.RefreshScore(false);
    }

    public override void OnEvent(SFX_Event evnt)
    {
        if (evnt.SoundName == "BuoyPop")
        {
            AudioDuck.Instance.PlaySound(AudioDuck.Instance.BuoyPop, gameObject);
        }
    }
}