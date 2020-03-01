using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInfo PlayerInfo;

    internal PlayerControl PlayerControl;
    internal PlayerCostume PlayerCostume;

    public float Radius = 1f;
    public float MaxSpeed = 2f;
    public float Accelerate = 2f;

    void Awake()
    {
        PlayerControl = GetComponent<PlayerControl>();
        PlayerCostume = GetComponent<PlayerCostume>();
    }

    public static Player BaseInitialize(PlayerInfo playerInfo)
    {
        GameObject playerPrefab = PrefabManager.Instance.GetPrefab("Player_" + playerInfo.PlayerType);
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

    public Vector3 GetPlayerPosition => PlayerControl.PlayerMove.transform.position;

    public void SetPlayerPosition(Vector3 pos)
    {
        PlayerControl.PlayerMove.transform.position = pos;
    }

    public ParticleSystem ParticleSystem;

    public void Reset()
    {
    }
}

public enum PlayerNumber
{
    Player1 = 0,
    Player2 = 1,
    AnyPlayer = 16,
    AI = 99,
}

public enum PlayerType
{
    ArmSpringHammer = 0,
    RotatingProtector = 1,
    ForkLift = 2,
}