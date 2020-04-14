using Bolt;
using UnityEngine;

public class Feet : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    [SerializeField] private Animator Anim;
    public float FeetMoveThreshold = 1f;

    public enum MoveStates
    {
        Static,
        Moving
    }

    private MoveStates _moveState;

    private MoveStates MoveState
    {
        get { return _moveState; }
        set
        {
            if (_moveState != value)
            {
                switch (value)
                {
                    case MoveStates.Static:
                    {
                        Duck.Body.BodyAnimator.SetFloat("Gasp", 1.0f);
                        break;
                    }
                    case MoveStates.Moving:
                    {
                        Duck.Body.BodyAnimator.SetFloat("Gasp", 0f);
                        break;
                    }
                }

                _moveState = value;
            }
        }
    }

    void LateUpdate()
    {
        if (Duck.DuckRigidbody.velocity.magnitude < DuckConfig.BrakeVelocityThreshold)
        {
            Duck.DuckRigidbody.velocity *= 0.9f;
        }

        Vector3 vel = Duck.DuckRigidbody.velocity;
        float backForth = Vector3.Dot(vel, Duck.Body.BodyRotate.transform.forward);
        float leftRight = Vector3.Dot(vel, Duck.Body.BodyRotate.transform.right);

        Anim.SetFloat("LeftRight", leftRight);
        Anim.SetFloat("BackForth", backForth);

        // Walk
        if (Mathf.Abs(vel.magnitude) > FeetMoveThreshold)
        {
            Duck.Body.BodyAnimator.SetFloat("Breath", 0.1f);
            Duck.Body.BodyAnimator.SetFloat("Walk", 1.0f);
            Duck.Ring.Walking();
            Duck.Wings.Walking();
        }
        else // Stop
        {
            Duck.Body.BodyAnimator.SetFloat("Walk", 0f);
            MoveState = MoveStates.Static;
            Duck.Ring.NotWalking();
            Duck.Wings.NotWalking();
        }
    }
}