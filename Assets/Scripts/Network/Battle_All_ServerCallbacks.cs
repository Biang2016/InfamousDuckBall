using System.Linq;
using Bolt;
using UdpKit;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Battle_FlagRace", "Battle_Smash")]
public class Battle_All_ServerCallbacks : GlobalEventListener
{
    private static BoltEntity gameStateGO;

    void Awake()
    {
        if (PlayerObjectRegistry_Online.MyPlayer == null)
        {
            PlayerObjectRegistry_Online.CreateServerPlayer();
        }

        if (gameStateGO == null)
        {
            gameStateGO = BoltNetwork.Instantiate(BoltPrefabs.GameState, Vector3.zero, Quaternion.identity);
            gameStateGO.transform.SetParent(transform);
        }
    }

    public override void Connected(BoltConnection connection)
    {
        PlayerObjectRegistry_Online.CreateClientPlayer(connection);
    }

    public override void SceneLoadLocalDone(string map)
    {
        if (PlayerObjectRegistry_Online.ServerPlayer.Player == null)
        {
            PlayerObjectRegistry_Online.ServerPlayer.Spawn();
        }

        PlayerObjectRegistry_Online.MyPlayer = PlayerObjectRegistry_Online.ServerPlayer.Player;
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerObject player = PlayerObjectRegistry_Online.GetPlayer(connection);
        if (player.Player == null)
        {
            player.Spawn();
        }

        UpdatePlayerCountEvent evnt = UpdatePlayerCountEvent.Create();
        evnt.PlayerCount = BoltNetwork.Connections.Count() + 1;
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