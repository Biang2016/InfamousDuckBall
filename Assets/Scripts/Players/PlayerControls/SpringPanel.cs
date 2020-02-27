using System.Collections;
using UnityEngine;

public class SpringPanel : Controllable
{
    [SerializeField] private Animator Anim;
    [SerializeField] private float Force;
    [SerializeField] private float LogicKickDelay = 0.2f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
    }

    protected override void Operate_AI()
    {
    }

    void OnTriggerEnter(Collider c)
    {
        if (IsAI)
        {
            GoalBall ball = c.gameObject.GetComponent<GoalBall>();
            if (ball)
            {
                Anim.SetTrigger("Bump");
                StartCoroutine(Co_Bump(ball));
            }
        }
    }

    IEnumerator Co_Bump(GoalBall ball)
    {
        yield return new WaitForSeconds(LogicKickDelay);
        ball.GetRigidbody().AddForce((ball.transform.position - transform.position).normalized * Force);
    }
}