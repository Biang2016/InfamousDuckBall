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
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (Duck.DuckRigidbody.velocity.magnitude < DuckConfig.BrakeVelocityThreshold)
            {
                Duck.DuckRigidbody.velocity *= 0.9f;
            }
        }

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (!Player.entity.IsControllerOrOwner)
            {
                float _backForth = Vector3.Dot(Player.state.Velocity, Duck.Body.BodyRotate.transform.forward);
                float _leftRight = Vector3.Dot(Player.state.Velocity, Duck.Body.BodyRotate.transform.right);

                Anim.SetFloat("BackForth", _backForth);
                Anim.SetFloat("LeftRight", _leftRight);
            }
            else
            {
                float _backForth = Vector3.Dot(Duck.DuckRigidbody.velocity, Duck.Body.BodyRotate.transform.forward);
                float _leftRight = Vector3.Dot(Duck.DuckRigidbody.velocity, Duck.Body.BodyRotate.transform.right);

                Anim.SetFloat("BackForth", _backForth);
                Anim.SetFloat("LeftRight", _leftRight);
            }
        }
        else
        {
            float backForth = Vector3.Dot(Duck.DuckRigidbody.velocity, Duck.Body.BodyRotate.transform.forward);
            float leftRight = Vector3.Dot(Duck.DuckRigidbody.velocity, Duck.Body.BodyRotate.transform.right);

            Anim.SetFloat("BackForth", backForth);
            Anim.SetFloat("LeftRight", leftRight);
        }

        // Walk
        if (Mathf.Abs(Duck.DuckRigidbody.velocity.magnitude) > FeetMoveThreshold)
        {
            AudioDuck.Instance.StartPlayerMovementSound(Player.PlayerNumber, transform, Player.Duck.DuckRigidbody);
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

        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            MyPlayerCircle.enabled = Player.entity.HasControl;
            if (BoltNetwork.IsServer)
            {
                Player.state.Velocity = Duck.DuckRigidbody.velocity;
            }
        }
        else
        {
            MyPlayerCircle.enabled = true;
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