using UnityEngine;

public class RotatingBody : MonoBehaviour
{
    [SerializeField] private float ReactionTime = 1;
    [SerializeField] private float IdleRotateSpeed = 2;
    [SerializeField] private float ReactionDistance = 2f;

    [SerializeField] private float idleRotateInterval = 2f;
    private float idleRotateTick = 0;
    private float targetAngle;

    void Start()
    {
        idleRotateTick = idleRotateInterval;
    }

    void Update()
    {
        Vector3 distanceToBall = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        if (distanceToBall.magnitude < ReactionDistance)
        {
            distanceToBall = Vector3.Scale(new Vector3(1, 0, 1), distanceToBall);
            float angle = Vector3.SignedAngle(distanceToBall, Vector3.forward, Vector3.down);
            RotateToAngle(angle, ReactionTime);
            idleRotateTick = 0;
        }
        else
        {
            idleRotateTick += Time.deltaTime;
            if (idleRotateTick > idleRotateInterval)
            {
                idleRotateTick = 0;
                targetAngle = Random.Range(0, 360);
            }

            RotateToAngle(targetAngle, IdleRotateSpeed);
        }
    }

    public void RotateToAngle(float angle, float reactionTime)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.up * angle), Time.deltaTime * reactionTime);
    }
}