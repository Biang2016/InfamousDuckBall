using UnityEngine;

public class PlayerObject
{
    public PlayerNumber PlayerNumber;
    public TeamNumber TeamNumber;
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
            Character = BoltNetwork.Instantiate(BoltPrefabs.Player, new Vector3(0, 2, 0), Quaternion.identity);
            Player = Character.GetComponent<Player>();
            Player.Initialize(PlayerNumber, TeamNumber);

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