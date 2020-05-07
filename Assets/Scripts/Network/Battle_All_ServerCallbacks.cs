using Bolt;
using UdpKit;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Battle_FlagRace", "Battle_Smash")]
public class Battle_All_ServerCallbacks : Bolt.GlobalEventListener
{
    private static BoltEntity gameStateGO;

    void Awake()
    {
        if (PlayerObjectRegistry.MyPlayer == null)
        {
            PlayerObjectRegistry.CreateServerPlayer();
        }

        if (gameStateGO == null)
        {
            gameStateGO = BoltNetwork.Instantiate(BoltPrefabs.GameState, Vector3.zero, Quaternion.identity);
            gameStateGO.transform.SetParent(transform);
        }
    }

    public override void Connected(BoltConnection connection)
    {
        PlayerObjectRegistry.CreateClientPlayer(connection);
    }

    public override void SceneLoadLocalDone(string map)
    {
        if (PlayerObjectRegistry.ServerPlayer.Player == null)
        {
            PlayerObjectRegistry.ServerPlayer.Spawn();
        }

        PlayerObjectRegistry.MyPlayer = PlayerObjectRegistry.ServerPlayer.Player;
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerObject player = PlayerObjectRegistry.GetPlayer(connection);
        if (player.Player == null)
        {
            player.Spawn();
        }

        UpdatePlayerCountEvent evnt = UpdatePlayerCountEvent.Create();
        evnt.PlayerCount = GameManager.Instance.Cur_BallBattleManager.PlayerDict.Count;
        evnt.Send();
    }

    public override void Disconnected(BoltConnection connection)
    {
        base.Disconnected(connection);
        UpdatePlayerCountEvent evnt = UpdatePlayerCountEvent.Create();
        evnt.PlayerCount = GameManager.Instance.Cur_BallBattleManager.PlayerDict.Count;
        evnt.Send();
    }
}