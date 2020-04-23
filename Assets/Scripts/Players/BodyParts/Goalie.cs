using UnityEngine;

public class Goalie : MonoBehaviour
{
    [SerializeField] private Collider GoalCollider;

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
        FXManager.Instance.PlayFX(FX_Type.ScoreRingExplosion, transform.position, Quaternion.Euler(-90, 0, 0));
    }
}