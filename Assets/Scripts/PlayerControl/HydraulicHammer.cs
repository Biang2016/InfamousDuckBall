using UnityEngine;

public class HydraulicHammer : MonoBehaviour, IPlayerControl
{
    private PlayerNumber PlayerNumber;

    public void SetPlayerNumber(PlayerNumber playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    [SerializeField] private JoystickButton Button = JoystickButton.None;
    public Animator Anim;

    void FixedUpdate()
    {
        if (Button != JoystickButton.None)
        {
            if (Input.GetButtonDown(Button + "_" + PlayerNumber))
            {
                Bump();
            }
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(GameManager.Instance.Ball.transform);
    }

    public float KickRadius = 1.5f;
    public float Force = 100f;

    public void Bump()
    {
        Anim.SetTrigger("Kick");
        IKickable ko = GameManager.Instance.Ball;
        Vector3 diff = ko.GetRigidbody().transform.position - transform.position;
        float distance = diff.magnitude;
        if (distance < KickRadius)
        {
            ko.GetRigidbody().AddForce((diff.normalized + Vector3.up) * Force);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }
}