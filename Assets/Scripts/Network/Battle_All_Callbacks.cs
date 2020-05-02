using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[BoltGlobalBehaviour("Battle_Smash", "Battle_FlagRace")]
public class Battle_All_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(BattleEndEvent evnt)
    {
        if (GameManager.Instance.Cur_BallBattleManager)
        {
            if (GameManager.Instance.Cur_BallBattleManager.BattleType == (BattleTypes) evnt.BattleType)
            {
                GameManager.Instance.Cur_BallBattleManager.EndBattle();
            }
        }
    }

    public override void Disconnected(BoltConnection connection)
    {
        base.Disconnected(connection);
        PlayerObjectRegistry.RemovePlayer(connection);
    }

    public override void ControlOfEntityGained(BoltEntity entity)
    {
        PlayerObjectRegistry.MyPlayer = entity.GetComponent<Player>();
    }

    public override void OnEvent(CloseRoomEvent evnt)
    {
        if (GameManager.Instance.Cur_BallBattleManager)
        {
            if (BoltNetwork.IsClient)
            {
                GameManager.Instance.ReturnToLobby();
            }

            if (BoltNetwork.IsServer)
            {
                GameManager.Instance.Cur_BattleManager.IsClosing = true;
                PlayerObjectRegistry.RemoveAllPlayers();
                if (ReturnToLobby_ServerCoroutine == null)
                {
                    ReturnToLobby_ServerCoroutine = StartCoroutine(Co_ReturnToLobby_Server());
                }
            }
        }
    }

    private static Coroutine ReturnToLobby_ServerCoroutine;

    IEnumerator Co_ReturnToLobby_Server()
    {
        while (BoltNetwork.Clients.ToList().Count != 0)
        {
            yield return new WaitForSeconds(0.2f);
            CloseRoomEvent.Create().Send();
        }

        ReturnToLobby_ServerCoroutine = null;
        GameManager.Instance.ReturnToLobby();
    }
}