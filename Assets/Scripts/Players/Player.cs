using System;
using Bolt;
using UnityEngine;

public class Player : EntityBehaviour<IPlayerState>
{
    public PlayerNumber PlayerNumber => (PlayerNumber) state.PlayerInfo.PlayerNumber;
    public TeamNumber TeamNumber => (TeamNumber) state.PlayerInfo.TeamNumber;

    public PlayerController PlayerController;
    public PlayerCostume PlayerCostume;

    public Controller Controller
    {
        get => PlayerController.Controller;
        set => PlayerController.Controller = value;
    }

    public Duck Duck;
    public Goalie Goalie;
    public DuckConfig DuckConfig;
    public PlayerCollider PlayerCollider;

    public override void Attached()
    {
        PlayerController.Attached();
        Duck.Attached();
    }

    public void Initialize(PlayerNumber playerNumber, TeamNumber teamNumber)
    {
        state.PlayerInfo.PlayerNumber = (int) playerNumber;
        state.PlayerInfo.TeamNumber = (int) teamNumber;
        if (entity.IsOwner)
        {
            PlayerCostume.Initialize(playerNumber, teamNumber);
            Goalie.GoalIndicator.SetActive(false);
            PlayerCollider.Initialize(this);
            Duck.Initialize();
        }
    }

    public override void ControlGained()
    {
        if (!entity.IsOwner)
        {
            PlayerCostume.Initialize(PlayerNumber, TeamNumber);
            Goalie.GoalIndicator.SetActive(false);
            PlayerCollider.Initialize(this);
            Duck.Initialize();
        }
    }

    public void Reviving()
    {
    }

    public override void SimulateOwner()
    {
    }

    public override void SimulateController()
    {
        PlayerController.SimulateController();
        Duck.Body.SimulateController();
    }

    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerController.ExecuteCommand(command, resetState);
    }

    private void LateUpdate()
    {
        if (entity.HasControl && Controller == null)
        {
            if (MultiControllerManager.Instance.PlayerControlMap.ContainsKey(PlayerNumber))
            {
                Controller = MultiControllerManager.Instance.Controllers[MultiControllerManager.Instance.PlayerControlMap[PlayerNumber]];
            }
        }
    }

    public Vector3 GetPlayerPosition => Duck.Feet.transform.position;

    public void SetPlayerPosition(Vector3 pos)
    {
        Duck.Feet.transform.position = pos;
    }

    public void Reset()
    {
        Duck.DuckRigidbody.velocity = Vector3.zero;
        Duck.DuckRigidbody.angularVelocity = Vector3.zero;
    }
}