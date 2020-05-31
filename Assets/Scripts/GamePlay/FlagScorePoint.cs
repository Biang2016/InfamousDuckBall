using System.Collections.Generic;
using UnityEngine;

public class FlagScorePoint : MonoBehaviour
{
    public TeamNumber MyTeamNumber;
    public Animator Anim;

    void Awake()
    {
        Anim.SetFloat("Delay", Random.Range(0f, 1f));
    }

    private SortedDictionary<PlayerNumber, float> playerStayDurationDict = new SortedDictionary<PlayerNumber, float>();

    void OnTriggerEnter(Collider c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponentInParent<PlayerCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (p.TeamNumber == MyTeamNumber)
                {
                    if (p.HasRing)
                    {
                        AudioDuck.Instance.StartPlayerChargeSound(p.PlayerNumber, p.transform, p.Duck.DuckRigidbody);
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponentInParent<PlayerCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (p.TeamNumber == MyTeamNumber)
                {
                    if (p.HasRing)
                    {
                        if (!playerStayDurationDict.ContainsKey(p.PlayerNumber))
                        {
                            playerStayDurationDict.Add(p.PlayerNumber, 0f);
                        }

                        playerStayDurationDict[p.PlayerNumber] += Time.deltaTime;
                        if (playerStayDurationDict[p.PlayerNumber] > 2f)
                        {
                            playerStayDurationDict[p.PlayerNumber] = 0f;
                            ((BattleManager_FlagRace) GameManager.Instance.Cur_BallBattleManager).FlagScorePointHit_Server(p);
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponentInParent<PlayerCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (p.TeamNumber == MyTeamNumber)
                {
                    if (p.HasRing)
                    {
                        if (!playerStayDurationDict.ContainsKey(p.PlayerNumber))
                        {
                            playerStayDurationDict.Add(p.PlayerNumber, 0f);
                        }

                        playerStayDurationDict[p.PlayerNumber] = 0f;
                        AudioDuck.Instance.StopPlayerChargeSound(p.PlayerNumber);
                    }
                }
            }
        }
    }
}