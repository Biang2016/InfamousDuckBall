using UnityEngine;

public class BallValidZone : MonoBehaviour
{
    public Collider Collider;

    void OnTriggerExit(Collider c)
    {
        Ball ball = c.GetComponent<Ball>();
        ball?.ResetBall();
    }
}