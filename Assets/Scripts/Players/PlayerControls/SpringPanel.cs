using System.Collections;
using UnityEngine;

public class SpringPanel : MonoBehaviour, IPlayerControl
{
    private PlayerControl ParentPlayerControl;
    [SerializeField] private Animator Anim;
    [SerializeField] private float Force;
    [SerializeField] private float LogicKickDelay = 0.2f;

    public void Initialize(PlayerControl parentPlayerControl)
    {
        ParentPlayerControl = parentPlayerControl;
    }

    void OnTriggerEnter(Collider c)
    {
        GoalBall ball = c.gameObject.GetComponent<GoalBall>();
        if (ball)
        {
            Anim.SetTrigger("Bump");
            StartCoroutine(Co_Bump(ball));
        }
    }

    IEnumerator Co_Bump(GoalBall ball)
    {
        yield return new WaitForSeconds(LogicKickDelay);
        ball.GetRigidbody().AddForce((ball.transform.position - transform.position).normalized * Force);
    }
}