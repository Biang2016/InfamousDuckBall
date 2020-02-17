using System.Collections;
using UnityEngine;

public class GoalBall : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        transform.localScale *= 1.3f;
    }
}
