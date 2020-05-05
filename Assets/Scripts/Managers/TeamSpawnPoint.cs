using UnityEngine;

public class TeamSpawnPoint : MonoBehaviour, IRevivePlayer
{
    internal bool ConsiderInCamera = true;
    [SerializeField] private TeamNumber allowedTeamNumber;
    public TeamNumber AllowedTeamNumber => allowedTeamNumber;

    public void Init()
    {
    }

    void Update()
    {
    }

    public void Spawn(PlayerNumber playerNumber, TeamNumber teamNumber)
    {
        Player player = GameManager.Instance.Cur_BattleManager.GetPlayer(playerNumber);
        player.SetPlayerPosition(transform.position);
        player.Reviving();
    }
}