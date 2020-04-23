using System.Collections.Generic;
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
                EndBattle_Server();
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
    public abstract void EndBattle_Server();

    public virtual void EndBattle()
    {
        StopAllCoroutines();
        NoticeManager.Instance.ShowInfoPanelTop("GAME OVER!", 0, 0.7f);
    }
}