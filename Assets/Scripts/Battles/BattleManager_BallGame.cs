using UnityEngine;

public abstract class BattleManager_BallGame : BattleManager
{
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
                EndBattle_Server(TeamNumber.None);
            }
            else
            {
                StartBattle_Server();
            }
        }
    }

    public abstract void StartBattle_Server();

    public virtual void StartBattle()
    {
        NoticeManager.Instance.ShowInfoPanelTop("GAME START!", 0, 0.7f);
    }

    public abstract void BallHit_Server(Ball ball, Player hitPlayer, TeamNumber hitTeamNumber);
    public abstract void EndBattle_Server(TeamNumber winnerTeam);

    public virtual void EndBattle(TeamNumber winnerTeam, int team1Score, int team2Score)
    {
        StopAllCoroutines();
    }
}