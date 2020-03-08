using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInfo PlayerInfo;

    internal PlayerControl PlayerControl;
    internal PlayerCostume PlayerCostume;

    void Awake()
    {
        PlayerControl = GetComponent<PlayerControl>();
        PlayerCostume = GetComponent<PlayerCostume>();
    }

    public static Player BaseInitialize(PlayerInfo playerInfo)
    {
        GameObject playerPrefab = PrefabManager.Instance.GetPrefab("Goose");
        GameObject playerGO = Instantiate(playerPrefab);
        Player player = playerGO.GetComponent<Player>();
        player.Initialize(playerInfo);
        return player;
    }

    internal int Score = 0;

    public void Initialize(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        PlayerCostume.Initialize(PlayerInfo.PlayerNumber);
        PlayerControl.Initialize(this);
    }

    public bool ConsiderPlayerInCamera;

    public void Reviving(bool considerInCamera)
    {
        ConsiderPlayerInCamera = considerInCamera;
        PlayerControl.Controllable = true;
    }

    public Vector3 GetPlayerPosition => PlayerControl.Goose.transform.position;

    public void SetPlayerPosition(Vector3 pos)
    {
        PlayerControl.Goose.transform.position = pos;
    }

    public ParticleSystem ParticleSystem;

    public void Reset()
    {
        PlayerControl.PlayerRigidbody.velocity = Vector3.zero;
        PlayerControl.PlayerRigidbody.angularVelocity = Vector3.zero;
    }
}

public enum PlayerNumber
{
    Player1 = 0,
    Player2 = 1,
    Player3 = 2,
    Player4 = 3,
    AnyPlayer = 16,
    AI = 99,
}