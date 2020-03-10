using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInfo PlayerInfo;

    internal PlayerControl PlayerControl;
    internal PlayerCostume PlayerCostume;
    public GameObject GoalIndicator;
    public Collider GoalCollider;

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

    public void Initialize(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        PlayerCostume.Initialize(PlayerInfo.PlayerNumber, playerInfo.TeamNumber);
        PlayerControl.Initialize(this);
        GoalIndicator.SetActive(false);
    }

    public bool ConsiderPlayerInCamera;

    public void Reviving(bool considerInCamera)
    {
        ConsiderPlayerInCamera = considerInCamera;
        PlayerControl.Controllable = true;
    }

    public Vector3 GetPlayerPosition => PlayerControl.Goose.Feet.transform.position;

    public void SetPlayerPosition(Vector3 pos)
    {
        PlayerControl.Goose.Feet.transform.position = pos;
    }

    public ParticleSystem ParticleSystem;

    public void Reset()
    {
        PlayerControl.PlayerRigidbody.velocity = Vector3.zero;
        PlayerControl.PlayerRigidbody.angularVelocity = Vector3.zero;
    }

    public void SwitchTeam(int increase)
    {
        GameManager.Instance.TeamDict[PlayerInfo.TeamNumber].TeamPlayers.Remove(this);
        PlayerInfo.TeamNumber = (TeamNumber) ((((int) PlayerInfo.TeamNumber) + increase + GameManager.TeamNumberCount) % GameManager.TeamNumberCount);
        PlayerCostume.Initialize(PlayerInfo.PlayerNumber, PlayerInfo.TeamNumber);
        GameManager.Instance.TeamDict[PlayerInfo.TeamNumber].TeamPlayers.Add(this);
        GameManager.Instance.RefreshAllTeamGoal();
        UIManager.Instance.GetBaseUIForm<DebugPanel>().RefreshScore();
    }

    private bool isAGoal = false;

    public bool IsAGoal
    {
        get { return isAGoal; }
        set
        {
            isAGoal = value;
            GoalIndicator.SetActive(value);
            GoalCollider.isTrigger = value;
        }
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