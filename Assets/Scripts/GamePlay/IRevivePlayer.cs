public interface IRevivePlayer
{
    TeamNumber AllowedTeamNumber { get; }
    void Init();
    void Spawn(PlayerNumber playerNumber, TeamNumber teamNumber);
}