using System.Collections.Generic;
using UnityEngine;

public abstract class BattleManager_BallGame : BattleManager
{
    internal Ball Ball
    {
        get
        {
            if (!ball)
            {
                ball = FindObjectOfType<Ball>();
            }

            return ball;
        }
    }

    protected Ball ball;
    internal Vector3 BallDefaultPos = Vector3.zero;
    public Transform BallPivot;

    public override void Child_Initialize()
    {
        if (BoltNetwork.IsServer)
        {
            ResetAllPlayers();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.F10))
        {
            if (IsStart)
            {
                EndBattle_Server();
            }
            else
            {
                StartBattle_Server();
            }
        }

        if (IsStart && BoltNetwork.IsServer)
        {
            if (ball)
            {
                ball.RigidBody.mass = GameManager.Instance.GameState.state.DuckConfig.BallWeight * ConfigManager.Instance.BallWeight;
                ball.Collider.material.bounciness = GameManager.Instance.GameState.state.DuckConfig.BallBounce * ConfigManager.Instance.BallBounce;
            }
        }
    }

    public abstract void StartBattle_Server();

    public virtual void StartBattle()
    {
        NoticeManager.Instance.ShowInfoPanelTop("GAME START!", 0, 0.7f);
    }

    public abstract void BallHit_Server(Player hitPlayer, TeamNumber hitTeamNumber);
    public abstract void ResetBall();
    public abstract void EndBattle_Server();

    public virtual void EndBattle()
    {
        StopAllCoroutines();
        NoticeManager.Instance.ShowInfoPanelTop("GAME OVER!", 0, 0.7f);
    }
}