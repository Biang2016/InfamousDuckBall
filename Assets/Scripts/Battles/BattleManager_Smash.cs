using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BattleManager_Smash : BattleManager_BallGame
{
    public ScoreRingManager ScoreRingManager => Boat.ScoreRingManager;
    public Boat Boat;

    public override void Child_Initialize()
    {
        base.Child_Initialize();
        if (BoltNetwork.IsServer)
        {
            BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boat.ScoreRingsPivot.position, Boat.ScoreRingsPivot.rotation);
            Boat.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
        }

        GameManager.Instance.DebugPanel.RefreshScore(true);
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to Start/Stop, F4/F5/F6 to switch game");
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 0, 0);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void StartBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            StartNewRound();
            ScoreRingManager.Reset();
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.MegaScore = 0;
            }
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        IsStart = true;
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to Start/Stop, F4/F5/F6 to switch game");
    }

    public void StartNewRound()
    {
        if (BoltNetwork.IsServer)
        {
            BattleStartEvent.Create().Send();
            if (!Ball)
            {
                BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot.position, BallPivot.rotation);
                BallDefaultPos = Ball.transform.position;
            }
            else
            {
                ResetBall();
            }

            ScoreRingManager.Reset();
            ResetAllPlayers();

            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.Score = ConfigManager.Smash_TeamStartScore;
                List<Player> goalies = ClientUtils.GetRandomFromList(kv.Value.TeamPlayers, 1);
                if (goalies.Count > 0)
                {
                    CostumeType ct = ScoreRingManager.GetRingCostumeType(kv.Key);
                    StartCoroutine(Co_PlayerRingRecover(goalies[0], ct));

                    ScoreChangeEvent sce = ScoreChangeEvent.Create();
                    sce.TeamNumber = (int) kv.Key;
                    sce.Score = kv.Value.Score;
                    sce.MegaScore = kv.Value.MegaScore;
                    sce.Send();
                }
            }
        }
    }

    public override void ResetBall()
    {
        if (BoltNetwork.IsServer)
        {
            StartCoroutine(Co_ResetBall(1f));
        }
    }

    IEnumerator Co_ResetBall(float suspendingTime)
    {
        if (Ball)
        {
            Ball.RigidBody.DOPause();
            Ball.transform.position = BallDefaultPos;
            Ball.Reset();
            Ball.RigidBody.useGravity = false;
            yield return new WaitForSeconds(suspendingTime);
            if (Ball) Ball.RigidBody.useGravity = true;
        }
    }

    public override void BallHit_Server(Player player, TeamNumber teamNumber)
    {
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = false;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.Send();
        player.state.HasRing = false;

        Team hitTeam = TeamDict[teamNumber];

        if (hitTeam.Score == 1)
        {
            TeamNumber otherTeam = teamNumber == TeamNumber.Team1 ? TeamNumber.Team2 : TeamNumber.Team1;
            if (TeamDict[otherTeam].MegaScore < ConfigManager.Smash_TeamTargetMegaScore - 1)
            {
                ScoreChangeEvent _sce = ScoreChangeEvent.Create();
                _sce.TeamNumber = (int) otherTeam;
                _sce.Score = TeamDict[otherTeam].Score;
                TeamDict[otherTeam].MegaScore += 1;
                _sce.MegaScore = TeamDict[otherTeam].MegaScore;
                _sce.Send();
                StartNewRound();
                return;
            }
            else
            {
                GameManager.Instance.DebugPanel.Wins(otherTeam);
                EndBattle_Server();
            }
        }
        else
        {
            CostumeType ct = ScoreRingManager.GetRingCostumeType(player.TeamNumber);
            Player otherPlayer = null;
            if (TeamDict[teamNumber].TeamPlayers.Count == 1)
            {
                otherPlayer = player;
            }
            else
            {
                List<Player> ps = ClientUtils.GetRandomFromList(TeamDict[teamNumber].TeamPlayers, 1, new List<Player> {player});
                otherPlayer = ps[0];
            }

            StartCoroutine(Co_PlayerRingRecover(otherPlayer, ct));
        }

        hitTeam.Score -= 1;
        ScoreChangeEvent sce = ScoreChangeEvent.Create();
        sce.TeamNumber = (int) teamNumber;
        sce.Score = hitTeam.Score;
        sce.MegaScore = hitTeam.MegaScore;
        sce.Send();

        ResetBall();
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
        pre.Send();
        player.state.HasRing = true;

        switch (player.TeamNumber)
        {
            case TeamNumber.Team1:
            {
                ScoreRingManager.state.RingNumber_Team1--;
                break;
            }
            case TeamNumber.Team2:
            {
                ScoreRingManager.state.RingNumber_Team2--;
                break;
            }
        }
    }

    public override void EndBattle_Server()
    {
        if (BoltNetwork.IsServer)
        {
            BattleEndEvent.Create().Send();
            if (Ball)
            {
                BoltNetwork.Destroy(Ball.gameObject);
            }

            ResetAllPlayers();
            ScoreRingManager.Reset();
            foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
            {
                kv.Value.Score = ConfigManager.Smash_TeamStartScore;
                kv.Value.MegaScore = 0;
                ScoreChangeEvent sce = ScoreChangeEvent.Create();
                sce.TeamNumber = (int) kv.Key;
                sce.Score = kv.Value.Score;
                sce.MegaScore = kv.Value.MegaScore;
                sce.Send();
            }
        }
    }

    public override void EndBattle()
    {
        base.EndBattle();
        ball = null;
        IsStart = false;
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F10 to Start/Stop, F4/F5/F6 to switch game");
        GameManager.Instance.DebugPanel.RefreshScore(true);
    }
}