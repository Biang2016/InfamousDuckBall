using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[BoltGlobalBehaviour("Battle_Smash", "Battle_FlagRace")]
public class Battle_All_Callbacks : Bolt.GlobalEventListener
{
    public override void OnEvent(BattleReadyStartToggleEvent evnt)
    {
        OnEvent_BattleReadyStartToggleEvent(evnt.Start, evnt.Tick);
    }

    public static void OnEvent_BattleReadyStartToggleEvent(bool start, int tick)
    {
        GameManager.Instance.Cur_BallBattleManager.StartBattleReadyToggle(start, tick);
    }

    public override void OnEvent(BattleStartEvent evnt)
    {
        OnEvent_BattleStartEvent();
    }

    public static void OnEvent_BattleStartEvent()
    {
        GameManager.Instance.Cur_BallBattleManager.StartBattle();
    }

    public override void OnEvent(BattleEndEvent evnt)
    {
        OnEvent_BattleEndEvent(evnt.BattleType, evnt.WinnerTeamNumber, evnt.Team1Score, evnt.Team2Score);
    }

    public static void OnEvent_BattleEndEvent(int battleType, int winnerTeamNumber, int team1Score, int team2Score)
    {
        if (GameManager.Instance.Cur_BallBattleManager)
        {
            if (GameManager.Instance.Cur_BallBattleManager.BattleType == (BattleTypes) battleType)
            {
                GameManager.Instance.Cur_BallBattleManager.EndBattle((TeamNumber) winnerTeamNumber, team1Score, team2Score);
            }
        }
    }

    public override void OnEvent(PlayerRingEvent evnt)
    {
        OnEvent_PlayerRingEvent(evnt.PlayerNumber, evnt.HasRing, evnt.CostumeType, evnt.Exploded);
    }

    public static void OnEvent_PlayerRingEvent(int playerNumber, bool hasRing, int costumeType, bool exploded)
    {
        Player player = GameManager.Instance.Cur_BattleManager.GetPlayer((PlayerNumber) playerNumber);
        if (hasRing)
        {
            player.GetRing((CostumeType) costumeType);
        }
        else
        {
            player.LoseRing(exploded);
        }
    }

    public override void OnEvent(UpdatePlayerCountEvent evnt)
    {
        OnEvent_UpdatePlayerCountEvent(evnt.PlayerCount);
    }

    public static void OnEvent_UpdatePlayerCountEvent(int playerCount)
    {
        if (GameManager.Instance.Cur_BallBattleManager)
        {
            GameManager.Instance.Cur_BallBattleManager.RefreshPlayerNumber(playerCount);
        }
    }

    public override void Disconnected(BoltConnection connection)
    {
        base.Disconnected(connection);
        PlayerObjectRegistry_Online.RemovePlayer(connection);
    }

    public override void ControlOfEntityGained(BoltEntity entity)
    {
        PlayerObjectRegistry_Online.MyPlayer = entity.GetComponent<Player>();
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
                PlayerObjectRegistry_Online.RemoveAllPlayers();
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
            CloseRoomEvent evnt = CloseRoomEvent.Create();
            evnt.Send();
        }

        ReturnToLobby_ServerCoroutine = null;
        GameManager.Instance.ReturnToLobby();
    }

    public override void OnEvent(BallSOSEvent evnt)
    {
        OnEvent_BallSOSEvent(evnt.Shown, evnt.BallName);
    }

    public static void OnEvent_BallSOSEvent(bool shown, string ballName)
    {
        if (GameManager.Instance.Cur_BattleManager)
        {
            switch (GameManager.Instance.Cur_BattleManager.BattleType)
            {
                case BattleTypes.Smash:
                {
                    Ball ball = ((BattleManager_Smash) GameManager.Instance.Cur_BattleManager).Ball;
                    ball?.SetSOSBubbleShown_Client(shown);
                    break;
                }
                case BattleTypes.FlagRace:
                {
                    Ball leftBall = ((BattleManager_FlagRace) GameManager.Instance.Cur_BattleManager).LeftBall;
                    Ball rightBall = ((BattleManager_FlagRace) GameManager.Instance.Cur_BattleManager).RightBall;
                    if (ballName == leftBall.BallName)
                    {
                        leftBall.SetSOSBubbleShown_Client(shown);
                    }

                    if (ballName == rightBall.BallName)
                    {
                        rightBall.SetSOSBubbleShown_Client(shown);
                    }

                    break;
                }
            }
        }
    }

    public override void OnEvent(SFX_Event evnt)
    {
        OnEvent_SFX_Event(evnt.SoundName);
    }

    public static void OnEvent_SFX_Event(string soundName)
    {
        if (GameManager.Instance.Cur_BattleManager)
        {
            AudioDuck.Instance.PlaySound(soundName, GameManager.Instance.Cur_BattleManager.BattleCamera.gameObject);
        }
    }
}