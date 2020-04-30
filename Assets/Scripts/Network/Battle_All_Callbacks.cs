using Bolt;

[BoltGlobalBehaviour("Battle_Prepare", "Battle_Smash", "Battle_FlagRace")]
public class Battle_All_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(BattleEndEvent evnt)
    {
        GameManager.Instance.Cur_BallBattleManager.EndBattle();
    }
}