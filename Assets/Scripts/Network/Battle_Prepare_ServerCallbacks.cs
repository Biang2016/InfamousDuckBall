using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Battle_Prepare")]
public class Battle_Prepare_ServerCallbacks : Bolt.GlobalEventListener
{
    void Awake()
    {
        PlayerObjectRegistry.CreateServerPlayer();

        BoltEntity gameStateGO = BoltNetwork.Instantiate(BoltPrefabs.GameState, Vector3.zero, Quaternion.identity);
        gameStateGO.transform.SetParent(transform);
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
    }
}