using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Client, "Battle_Prepare", "Battle_Smash", "Battle_FlagRace")]
public class Battle_All_ClientCallbacks : Bolt.GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
    }

    public override void ControlOfEntityGained(BoltEntity entity)
    {
        PlayerObjectRegistry.MyPlayer = entity.GetComponent<Player>();
    }

}