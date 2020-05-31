using System.Collections.Generic;
using UnityEngine;

public static class PlayerObjectRegistry_Online
{
    private static SortedDictionary<PlayerNumber, PlayerObject> playerDict = new SortedDictionary<PlayerNumber, PlayerObject>();
    public static Player MyPlayer;

    public static PlayerObject ServerPlayer
    {
        get
        {
            if (playerDict.ContainsKey(PlayerNumber.Player1))
            {
                return playerDict[PlayerNumber.Player1];
            }
            else
            {
                return null;
            }
        }
    }

    public static void RemoveAllPlayers()
    {
        if (BoltNetwork.IsServer)
        {
            foreach (KeyValuePair<PlayerNumber, PlayerObject> kv in playerDict)
            {
                BoltNetwork.Destroy(kv.Value.Player.gameObject);
            }
        }

        playerDict.Clear();
        MyPlayer = null;
    }

    public static void RemovePlayer(BoltConnection connection)
    {
        PlayerObject player = null;
        foreach (KeyValuePair<PlayerNumber, PlayerObject> kv in playerDict)
        {
            if (kv.Value.Connection == null)
            {
                player = kv.Value;
            }
            else
            {
                if (kv.Value.Connection.ConnectionId == connection.ConnectionId)
                {
                    player = kv.Value;
                }
            }
        }

        if (player != null)
        {
            playerDict.Remove(player.PlayerNumber);
            BoltNetwork.Destroy(player.Player.gameObject);
        }
    }

    public static void CreateServerPlayer()
    {
        CreatePlayer(null, new PlayerInfoData(PlayerPrefs.GetString("PlayerID"), PlayerNumber.Player1, (TeamNumber) (playerDict.Count % 2), CostumeType.Costume1));
    }

    public static void CreateClientPlayer(BoltConnection connection)
    {
        PlayerNumber pn = FindUnusedPlayerNumber();
        if (pn != PlayerNumber.None && pn != PlayerNumber.Player1)
        {
            string playerName = "unknown";
            if (connection.ConnectToken is ClientConnectToken cct)
            {
                playerName = cct.UserName;
            }

            CreatePlayer(connection, new PlayerInfoData(playerName, pn, (TeamNumber) (playerDict.Count % 2), CostumeType.Costume1));
        }
    }

    private static void CreatePlayer(BoltConnection connection, PlayerInfoData playerInfoData)
    {
        if (playerDict.ContainsKey(playerInfoData.PlayerNumber)) return;
        PlayerObject player = new PlayerObject();
        player.Connection = connection;
        if (player.Connection != null)
        {
            player.Connection.UserData = player;
        }

        player.PlayerName = playerInfoData.PlayerName;
        player.PlayerNumber = playerInfoData.PlayerNumber;
        player.TeamNumber = playerInfoData.TeamNumber;
        player.CostumeType = playerInfoData.CostumeType;

        playerDict.Add(playerInfoData.PlayerNumber, player);
    }

    private static PlayerNumber FindUnusedPlayerNumber()
    {
        for (int i = 0; i < ConfigManager.MaxPlayerNumber_Local; i++)
        {
            PlayerNumber pn = (PlayerNumber) i;
            if (!playerDict.ContainsKey(pn))
            {
                return pn;
            }
        }

        return PlayerNumber.None;
    }

    public static PlayerObject GetPlayer(BoltConnection connection)
    {
        if (connection == null)
        {
            return ServerPlayer;
        }

        return (PlayerObject) connection.UserData;
    }
}