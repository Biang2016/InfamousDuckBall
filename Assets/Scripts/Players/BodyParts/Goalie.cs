using UnityEngine;

public class Goalie : MonoBehaviour
{
    [SerializeField] private Collider GoalCollider;
    [SerializeField] private ParticleSystem ParticleSystem;

    [SerializeField] private bool isGoalie = false;

    public bool IsGoalie
    {
        get { return isGoalie; }
        set
        {
            isGoalie = value;
            GoalCollider.isTrigger = value;
        }
    }

    public void ParticleRelease()
    {
        ParticleSystem.Play();
    }
}