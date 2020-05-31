using System.Collections.Generic;
using UnityEngine;

public static class PlayerObjectRegistry_Local
{
    private static SortedDictionary<PlayerNumber, PlayerObject> playerDict = new SortedDictionary<PlayerNumber, PlayerObject>();
    private static List<string> localPlayerNames = new List<string> {"Steven", "Williams", "Julia", "Yunci", "Jingyi"};
    private static List<string> localPlayerNames_temp = new List<string>();

    public static void Init()
    {
        RemoveAllPlayers();
        localPlayerNames_temp.Clear();
        localPlayerNames_temp = ClientUtils.GetRandomFromList(localPlayerNames, 4);
    }

    public static void RemoveAllPlayers()
    {
        foreach (KeyValuePair<PlayerNumber, PlayerObject> kv in playerDict)
        {
            Object.Destroy(kv.Value.Player.gameObject);
        }

        playerDict.Clear();
        MultiControllerManager.Instance.PlayerControllerMap.Clear();
    }

    public static PlayerNumber CreatePlayer(ControllerIndex controllerIndex)
    {
        PlayerNumber pn = FindUnusedPlayerNumber();
        if (playerDict.ContainsKey(pn)) return PlayerNumber.None;
        PlayerObject player = new PlayerObject();
        if (player.Connection != null)
        {
            player.Connection.UserData = player;
        }

        player.PlayerName = localPlayerNames_temp[(int) pn];
        player.PlayerNumber = pn;
        player.TeamNumber = (TeamNumber) (playerDict.Count % 2);
        player.CostumeType = CostumeType.Costume1;
        playerDict.Add(pn, player);
        return pn;
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

    public static PlayerObject GetPlayer(PlayerNumber playerNumber)
    {
        playerDict.TryGetValue(playerNumber, out PlayerObject player);
        return player;
    }

    public static void SetAllPlayerControllerActive(bool active, bool active_RightStick_OR)
    {
        foreach (KeyValuePair<PlayerNumber, PlayerObject> kv in playerDict)
        {
            kv.Value.Player.Controller.Active = active;
            kv.Value.Player.Controller.Active_RightStick_OR = active_RightStick_OR;
        }
    }
}