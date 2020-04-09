using UnityEngine;

public class BallValidZone : MonoBehaviour
{
    void OnTriggerExit(Collider c)
    {
        Ball ball = c.GetComponent<Ball>();
        if (ball)
        {
            GameManager.Cur_BattleManager.ResetBall();
        }
    }
}