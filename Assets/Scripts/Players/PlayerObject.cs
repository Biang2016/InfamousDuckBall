using UnityEngine;

public class PlayerObject
{
    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
    public CostumeType CostumeType;
    public Player Player;
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

    public void Spawn()
    {
        if (!Character)
        {
            Character = BoltNetwork.Instantiate(BoltPrefabs.Player, Vector3.zero, Quaternion.identity);
            Player = Character.GetComponent<Player>();
            Player.Initialize_Server(PlayerNumber, TeamNumber, CostumeType);
            GameManager.Cur_BattleManager.AddPlayer(Player);
            GameManager.Cur_BattleManager.PlayerSpawnPointManager.Spawn(PlayerNumber);

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
}