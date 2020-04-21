using UnityEngine;

[BoltGlobalBehaviour("Battle_Prepare")]
public class Battle_Prepare_Callbacks : Bolt.GlobalEventListener
{
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