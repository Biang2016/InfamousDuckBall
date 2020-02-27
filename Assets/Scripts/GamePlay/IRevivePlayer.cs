public interface IRevivePlayer
{
    PlayerNumber AllowedPlayerNumber { get; }
    void Init();
    void Spawn(PlayerInfo playerInfo);
}