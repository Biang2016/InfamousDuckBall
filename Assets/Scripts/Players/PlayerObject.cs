using UnityEngine;

public class PlayerObject
{
    public string PlayerName;
    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
    public CostumeType CostumeType;
    public Player Player;

    #region OnlineMode

    public BoltEntity Character;
    public BoltConnection Connection;

    public bool IsServer
    {
        get { return Connection == null; }
    }

    public bool IsClient
    {
        get { return Connection != null; }
    }

    #endregion

    public void Spawn()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (!Character)
            {
                Character = BoltNetwork.Instantiate(BoltPrefabs.Player, Vector3.zero, Quaternion.identity);
                Player = Character.GetComponent<Player>();
                Player.Initialize_Server(PlayerName, PlayerNumber, TeamNumber, CostumeType);
                GameManager.Instance.Cur_BattleManager.AddPlayer(Player);
                GameManager.Instance.Cur_BattleManager.PlayerSpawnPointManager.Spawn(PlayerNumber, TeamNumber);

                if (IsServer)
                {
                    Character.TakeControl();
                }
                else
                {
                    Character.AssignControl(Connection);
                }
            }
        }
        else
        {
            if (!Player)
            {
                GameObject playerGO = Object.Instantiate(PrefabManager.Instance.GetPrefab("Player"), Vector3.zero, Quaternion.identity);
                Player = playerGO.GetComponent<Player>();
                Player.Initialize_Server(PlayerName, PlayerNumber, TeamNumber, CostumeType);
                GameManager.Instance.Cur_BattleManager.AddPlayer(Player);
                GameManager.Instance.Cur_BattleManager.PlayerSpawnPointManager.Spawn(PlayerNumber, TeamNumber);
            }
        }
    }
}