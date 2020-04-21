using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerObjectRegistry
{
    static SortedDictionary<PlayerNumber, PlayerObject> playerDict = new SortedDictionary<PlayerNumber, PlayerObject>();

    // create a player for a connection
    // note: connection can be null
    static void CreatePlayer(BoltConnection connection, PlayerInfoData playerInfoData)
    {
        if (playerDict.ContainsKey(playerInfoData.PlayerNumber)) return;
        PlayerObject player;

        // create a new player object, assign the connection property
        // of the object to the connection was passed in
        player = new PlayerObject();
        player.Connection = connection;

        // if we have a connection, assign this player
        // as the user data for the connection so that we
        // always have an easy way to get the player object
        // for a connection
        if (player.Connection != null)
        {
            player.Connection.UserData = player;
        }

        player.PlayerNumber = playerInfoData.PlayerNumber;
        player.TeamNumber = playerInfoData.TeamNumber;
        player.CostumeType = playerInfoData.CostumeType;

        // add to list of all players
        playerDict.Add(playerInfoData.PlayerNumber, player);
    }

    // this simply returns the 'players' list cast to
    // an IEnumerable<T> so that we hide the ability
    // to modify the player list from the outside.
    public static IEnumerable<PlayerObject> AllPlayers
    {
        get { return playerDict.Values; }
    }

    // finds the server player by checking the
    // .IsServer property for every player object.
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

    public static Player MyPlayer;

    public static void CreateServerPlayer()
    {
        CreatePlayer(null, new PlayerInfoData(PlayerNumber.Player1, (TeamNumber) (playerDict.Count % 2), CostumeType.Costume1));
    }

    public static void CreateServerPlayer(PlayerInfoData playerInfoData)
    {
        CreatePlayer(null, playerInfoData);
    }

    public static void CreateClientPlayer(BoltConnection connection)
    {
        PlayerNumber pn = FindUnusedPlayerNumber();
        if (pn != PlayerNumber.None && pn != PlayerNumber.Player1)
        {
            CreatePlayer(connection, new PlayerInfoData(pn, (TeamNumber) (playerDict.Count % 2), CostumeType.Costume1));
        }
    }

    public static void CreateClientPlayer(BoltConnection connection, PlayerInfoData playerInfoData)
    {
        CreatePlayer(connection, playerInfoData);
    }

    static PlayerNumber FindUnusedPlayerNumber()
    {
        for (int i = 0; i < ConfigManager.MaximalPlayerNumber; i++)
        {
            PlayerNumber pn = (PlayerNumber) i;
            if (!playerDict.ContainsKey(pn))
            {
                return pn;
            }
        }

        return PlayerNumber.None;
    }

    // utility function which lets us pass in a
    // BoltConnection object (even a null) and have
    // it return the proper player object for it.
    public static PlayerObject GetPlayer(BoltConnection connection)
    {
        if (connection == null)
        {
            return ServerPlayer;
        }

        return (PlayerObject) connection.UserData;
    }
}