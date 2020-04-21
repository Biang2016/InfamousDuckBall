using Bolt;
using UnityEngine;

public class Feet : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    [SerializeField] private SpriteRenderer MyPlayerCircle;
    [SerializeField] private SpriteRenderer ChargingCircle;

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

    private Vector3 lastPos = Vector3.zero;

    void LateUpdate()
    {
        MyPlayerCircle.enabled = Player.entity.HasControl;

        if (Duck.DuckRigidbody.velocity.magnitude < DuckConfig.BrakeVelocityThreshold)
        {
            Duck.DuckRigidbody.velocity *= 0.9f;
        }

        Vector3 vel = (transform.position - lastPos) * Application.targetFrameRate;
        lastPos = transform.position;

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

        if (startCharge)
        {
            ChargingCircle.color = new Color(1, 1, 1, ChargingCircle.color.a + 0.5f / (DuckConfig.PushChargeTimeMaxDuration * Application.targetFrameRate));
            float scale = ChargingCircle.transform.localScale.x - (1.5f / (DuckConfig.PushChargeTimeMaxDuration * Application.targetFrameRate));
            scale = Mathf.Clamp(scale, 0, 2);
            ChargingCircle.transform.localScale = Vector3.one * scale;
        }
    }

    private bool startCharge = false;

    public void StartCharge()
    {
        startCharge = true;
        ChargingCircle.color = new Color(1, 1, 1, 0.1f);
        ChargingCircle.transform.localScale = Vector3.one * 2f;
    }

    public void ReleaseChargingCircle()
    {
        startCharge = false;
        ChargingCircle.transform.localScale = Vector3.one * 2f;
        ChargingCircle.color = new Color(1, 1, 1, 0);
    }
}