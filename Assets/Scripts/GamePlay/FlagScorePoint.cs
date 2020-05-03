using UnityEngine;

public class FlagScorePoint : MonoBehaviour
{
    public TeamNumber MyTeamNumber;
    public Animator Anim;

    void Awake()
    {
        Anim.SetFloat("Delay", Random.Range(0f, 1f));
    }

    void OnTriggerEnter(Collider c)
    {
        if (BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponentInParent<PlayerCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (p.TeamNumber == MyTeamNumber)
                {
                    if (PlayerObjectRegistry.MyPlayer == p)
                    {
                        //Todo Vibrate
                    }

                    ((BattleManager_FlagRace) GameManager.Instance.Cur_BallBattleManager).FlagScorePointHit_Server(p);
                }
            }
        }
    }
}