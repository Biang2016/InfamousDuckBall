using UnityEngine;

public class PlayerValidZone : MonoBehaviour
{
    void OnTriggerExit(Collider c)
    {
        Player p = c.GetComponentInParent<Player>();
        if (p)
        {
            GameManager.Cur_BattleManager.ResetPlayer(p);
        }
    }
}