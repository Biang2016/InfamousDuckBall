using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_Smash : BattleManager_BallGame
{
    internal Ball Ball;
    public Transform BallPivot;
    internal Vector3 BallDefaultPos = Vector3.zero;
    public BallValidZone BallValidZone;

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
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F4/F5/F6 to switch game, F10 to Start/Stop");
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 0, 0);
    }

    protected override void Update()
    {
        base.Update();
        if (IsStart && BoltNetwork.IsServer)
        {
            if (Ball)
            {
                Ball.RigidBody.mass = GameManager.Instance.GameState.state.DuckConfig.BallWeight * ConfigManager.Instance.BallWeight;
                Ball.Collider.material.bounciness = GameManager.Instance.GameState.state.DuckConfig.BallBounce * ConfigManager.Instance.BallBounce;
            }
        }
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
        int round = 0;
        foreach (KeyValuePair<TeamNumber, Team> kv in TeamDict)
        {
            round += kv.Value.MegaScore;
        }

        UIManager.Instance.ShowUIForms<RoundPanel>().Show(round + 1);
        StartCoroutine(Co_StartNewRound());
    }

    public void StartNewRound()
    {
        if (BoltNetwork.IsServer)
        {
            BattleStartEvent.Create().Send();
            if (!Ball)
            {
                BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.Ball, BallPivot.position, BallPivot.rotation);
                Ball = be1.GetComponent<Ball>();
                Ball.state.BallName = "SmashBall";
                Ball.ResetTransform = BallPivot;
                BallDefaultPos = Ball.transform.position;
            }
            else
            {
                Ball.ResetBall();
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
                    sce.IsNewBattle = true;
                    sce.Send();
                }
            }
        }
    }

    IEnumerator Co_StartNewRound()
    {
        PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = false;
        yield return new WaitForSeconds(5f);
        PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = true;
    }

    public override void BallHit_Server(Ball ball, Player player, TeamNumber teamNumber)
    {
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = false;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.Exploded = true;
        pre.Send();

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
                _sce.IsNewBattle = false;
                _sce.Send();
                StartNewRound();
                return;
            }
            else
            {
                //TODO
                //GameManager.Instance.DebugPanel.Wins(otherTeam);
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
        sce.IsNewBattle = false;
        sce.Send();

        ball.KickedFly();
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.FishBreath, GameManager.Instance.gameObject);
    }

    IEnumerator Co_PlayerRingRecover(Player player, CostumeType costumeType)
    {
        yield return new WaitForSeconds(ConfigManager.Instance.RingRecoverTime);
        PlayerRingEvent pre = PlayerRingEvent.Create();
        pre.HasRing = true;
        pre.PlayerNumber = (int) player.PlayerNumber;
        pre.CostumeType = (int) costumeType;
        pre.Exploded = false;
        pre.Send();

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
            BattleEndEvent evnt = BattleEndEvent.Create();
            evnt.BattleType = (int) BattleTypes.Smash;
            evnt.Send();
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
                sce.IsNewBattle = true;
                sce.Send();
            }
        }
    }

    public override void EndBattle()
    {
        base.EndBattle();
        Ball = null;
        IsStart = false;
        GameManager.Instance.DebugPanel.SetStartTipShown(true, "F4/F5/F6 to switch game, F10 to Start/Stop");
        GameManager.Instance.DebugPanel.RefreshScore(true);
    }
}