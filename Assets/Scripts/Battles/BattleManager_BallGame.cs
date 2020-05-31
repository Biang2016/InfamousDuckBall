using UnityEngine;

public abstract class BattleManager_BallGame : BattleManager
{
    public override void Child_Initialize()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            ResetAllPlayers();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.F10))
        {
            if (IsStart || startBattleCoroutine != null)
            {
                EndBattle_Server(TeamNumber.None);
            }
            else
            {
                StartBattle_Server();
            }
        }
    }

    public abstract void StartBattle_Server();
    protected Coroutine startBattleCoroutine;

    public abstract void StartBattleReadyToggle(bool start, int tick);
    public abstract void RefreshPlayerNumber(int playerNumber);

    public virtual void StartBattle()
    {
    }

    public abstract void BallHit_Server(Ball ball, Player hitPlayer, TeamNumber hitTeamNumber);
    public abstract void EndBattle_Server(TeamNumber winnerTeam);

    public virtual void EndBattle(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        StopAllCoroutines();
    }
}