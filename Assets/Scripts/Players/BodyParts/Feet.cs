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

    public Transform LeftFeet;
    public Transform RightFeet;
    [SerializeField] private Animator FeetAnimator;
    public float FeetMoveThreshold = 1f;

    public enum MoveStates
    {
        Static,
        Forward,
        Backward,
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
                        FeetAnimator.SetTrigger("Static");
                        break;
                    }
                    case MoveStates.Forward:
                    {
                        Duck.Body.BodyAnimator.SetFloat("Gasp", 0f);

                        FeetAnimator.SetTrigger("Forward");
                        break;
                    }
                    case MoveStates.Backward:
                    {
                        Duck.Body.BodyAnimator.SetFloat("Gasp", 0f);
                        FeetAnimator.SetTrigger("Backward");
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
        float forward = Vector3.Dot(vel, Duck.Body.BodyRotate.transform.forward);
        if (forward > FeetMoveThreshold)
        {
            Duck.Body.BodyAnimator.SetFloat("Breath", 0.1f);
            Duck.Body.BodyAnimator.SetFloat("Walk", 1.0f);
            MoveState = MoveStates.Forward;
            LeftFeet.right = -vel;
            RightFeet.right = -vel;
        }
        else if (forward < -FeetMoveThreshold)
        {
            Duck.Body.BodyAnimator.SetFloat("Breath", 0.1f);
            Duck.Body.BodyAnimator.SetFloat("Walk", 1.0f);
            MoveState = MoveStates.Backward;
            LeftFeet.right = vel;
            RightFeet.right = vel;
        }
        else
        {
            Duck.Body.BodyAnimator.SetFloat("Walk", 0f);
            MoveState = MoveStates.Static;
        }
    }
}