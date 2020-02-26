using UnityEngine;

public class BallValidZone : MonoBehaviour
{
    void OnTriggerExit(Collider c)
    {
        GoalBall ball = c.GetComponent<GoalBall>();
        if (ball)
        {
            GameManager.Instance.Cur_BattleManager.ResetBall();
        }
    }
}