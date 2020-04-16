using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[BoltGlobalBehaviour("MainScene")]
public class GameCallBacks_Battle_PVP4 : Bolt.GlobalEventListener
{
    public override void OnEvent(BattleStartEvent evnt)
    {
        GameManager.Cur_BattleManager.StartGame();
    }

    public override void OnEvent(PlayerRingEvent evnt)
    {
        Player player = GameManager.Cur_BattleManager.GetPlayer((PlayerNumber) evnt.PlayerNumber);
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
        GameManager.Cur_BattleManager.TeamDict[tn].Score = evnt.Score;
        StartCoroutine(Co_ScoreRingDisappear(tn, evnt.Score));
        AudioManager.Instance.SoundPlay("sfx/Sound_Score");
        GameManager.DebugPanel.RefreshScore();
    }

    IEnumerator Co_ScoreRingDisappear(TeamNumber tn, int score)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
        GameManager.Cur_BattleManager.ScoreRingManager.SetTeamRingNumber(tn, score);
    }

    public override void OnEvent(PlayerTeamChangeEvent evnt)
    {
        TeamNumber newTeamNumber = (TeamNumber) evnt.TeamNumber;
        TeamNumber oldTeamNumber = (TeamNumber) evnt.OriTeamNumber;
        PlayerNumber pn = (PlayerNumber) evnt.PlayerNumber;
        Player player = GameManager.Cur_BattleManager.GetPlayer(pn);
        GameManager.Cur_BattleManager.TeamDict[oldTeamNumber].TeamPlayers.Remove(player);
        GameManager.Cur_BattleManager.TeamDict[newTeamNumber].TeamPlayers.Add(player);
        player.PlayerCostume.Initialize(pn, newTeamNumber, player.CostumeType);
    }

    public override void OnEvent(BattleEndEvent evnt)
    {
        GameManager.Cur_BattleManager.EndBattle();
    }
}