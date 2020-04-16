using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Client, "MainScene")]
public class ClientCallbacks_BattlePVP4 : Bolt.GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
    }

    public override void ControlOfEntityGained(BoltEntity entity)
    {
        PlayerObjectRegistry.MyPlayer = entity.GetComponent<Player>();
    }
}