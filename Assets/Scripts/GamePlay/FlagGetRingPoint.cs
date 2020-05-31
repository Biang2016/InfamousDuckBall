using UnityEngine;

public class FlagGetRingPoint : MonoBehaviour
{
    public TeamNumber MyTeamNumber;

    void OnTriggerEnter(Collider c)
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (c.gameObject.GetComponentInParent<PlayerCollider>())
            {
                Player p = c.GetComponentInParent<Player>();
                if (p.TeamNumber == MyTeamNumber)
                {
                    ((BattleManager_FlagRace) GameManager.Instance.Cur_BallBattleManager).GetFlagRing_Server(p);
                }
            }
        }
    }
}