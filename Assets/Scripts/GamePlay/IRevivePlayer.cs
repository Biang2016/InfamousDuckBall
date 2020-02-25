public interface IRevivePlayer
{
    PlayerNumber AllowedPlayerNumber { get; }
    float PlayerReviveInterval { get; }
    void Init();
    void AddRevivePlayer(PlayerNumber playerNumber, float time);
}