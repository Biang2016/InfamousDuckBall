using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "MainScene")]
public class ServerCallbacks : Bolt.GlobalEventListener
{
    void Awake()
    {
        PlayerObjectRegistry.CreateServerPlayer();
    }

    public override void Connected(BoltConnection connection)
    {
        PlayerObjectRegistry.CreateClientPlayer(connection);
    }

    public override void SceneLoadLocalDone(string map)
    {
        PlayerObjectRegistry.ServerPlayer.Spawn();
        PlayerObjectRegistry.MyPlayer = PlayerObjectRegistry.ServerPlayer.Player;
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerObjectRegistry.GetPlayer(connection).Spawn();
        BallEvent be = BallEvent.Create();
        be.Send();
    }
}