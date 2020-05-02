using System.Collections;
using Bolt;
using DG.Tweening;
using UnityEngine;

public class Ball : EntityEventListener<IBallState>
{
    public Collider Collider;
    public Rigidbody RigidBody;
    internal Transform ResetTransform;

    void OnTriggerEnter(Collider c)
    {
        if (BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponent<GoalCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (PlayerObjectRegistry.MyPlayer == p)
                {
                    //Todo Vibrate
                }

                GameManager.Instance.Cur_BallBattleManager.BallHit_Server(this, p, (TeamNumber) p.state.PlayerInfo.TeamNumber);
            }

            ScoreRingSingle srs = c.gameObject.GetComponentInParent<ScoreRingSingle>();
            if (srs)
            {
                srs.Explode(true);
                KickedFly();
            }
        }
    }

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
    }

    void Update()
    {
        if (GameManager.Instance.Cur_BattleManager != null)
        {
            if (GameManager.Instance.Cur_BattleManager is BattleManager_Smash smash)
            {
                if (smash.Ball == null)
                {
                    if (state.BallName == "FlagRaceBall_Left")
                    {
                        smash.Ball = this;
                    }
                }
            }

            if (GameManager.Instance.Cur_BattleManager is BattleManager_FlagRace flagRace)
            {
                if (flagRace.LeftBall == null)
                {
                    if (state.BallName == "FlagRaceBall_Left")
                    {
                        flagRace.LeftBall = this;
                    }
                }

                if (flagRace.RightBall == null)
                {
                    if (state.BallName == "FlagRaceBall_Right")
                    {
                        flagRace.RightBall = this;
                    }
                }
            }
        }
    }

    public void KickedFly()
    {
        transform.DOMoveY(10f, 0.3f);
        RigidBody.AddForce(Vector3.up * 800f);
    }

    public void ResetBall()
    {
        if (BoltNetwork.IsServer)
        {
            StartCoroutine(Co_ResetBall(1f));
        }
    }

    IEnumerator Co_ResetBall(float suspendingTime)
    {
        RigidBody.DOPause();
        transform.position = ResetTransform.position;
        RigidBody.velocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;
        RigidBody.useGravity = false;
        yield return new WaitForSeconds(suspendingTime);
        RigidBody.useGravity = true;
    }

    public void Kick(TeamNumber teamNumber, Vector3 force)
    {
        if (BoltNetwork.IsServer)
        {
            RigidBody.AddForce(force);
        }
    }
}