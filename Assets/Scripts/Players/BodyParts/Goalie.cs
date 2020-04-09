using UnityEngine;

public class Goalie : MonoBehaviour
{
    public GameObject GoalIndicator;
    public Collider GoalCollider;
    public ParticleSystem ParticleSystem;

    private bool isAGoalie = false;

    public bool IsAGoalie
    {
        get { return isAGoalie; }
        set
        {
            isAGoalie = value;
            GoalIndicator.SetActive(value);
            GoalCollider.isTrigger = value;
        }
    }
}