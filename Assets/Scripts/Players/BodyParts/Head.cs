using Bolt;
using UnityEngine;

public class Head : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    public void ExecuteCommand(bool leftTriggerDown, bool rightBumperDown, bool rightTriggerDown, bool rightTriggerUp, bool rightTriggerPressed)
    {
        switch (HeadStatus)
        {
            case HeadStatusTypes.Idle:
            {
                if (leftTriggerDown)
                {
                    Pull();
                }
                else if (rightTriggerDown)
                {
                    HeadStatus = HeadStatusTypes.PushCharging;
                }
                else if (rightTriggerPressed)
                {
                    HeadStatus = HeadStatusTypes.PushCharging;
                }
                else if (rightBumperDown)
                {
                    PushChargeForceRatio = 0f;
                    Push();
                }

                break;
            }
            case HeadStatusTypes.PushCharging:
            {
                if (rightTriggerUp)
                {
                    PushChargeForceRatio = PushChargeTimeTick / DuckConfig.PushChargeTimeMaxDuration;
                    PushChargeTimeTick = 0;
                    Push();
                }
                else if (rightTriggerPressed)
                {
                    PushChargeTimeTick += Time.deltaTime;
                    if (PushChargeTimeTick > DuckConfig.PushChargeTimeMaxDuration)
                    {
                        PushChargeForceRatio = PushChargeTimeTick / DuckConfig.PushChargeTimeMaxDuration;
                        PushChargeTimeTick = 0;
                        Push();
                    }
                }

                break;
            }
        }
    }

    public Collider HeadCollider;

    public void Initialize()
    {
        foreach (Collider c in transform.GetComponentsInChildren<Collider>())
        {
            if (c.gameObject.layer == GameManager.Layer_BallKicker)
            {
                string layerName = "BallKicker" + ((int) (Player.PlayerNumber) + 1);
                int layer = LayerMask.NameToLayer(layerName);
                c.gameObject.layer = layer;
            }
        }
    }

    public Animator Anim;

    private float PushChargeTimeTick = 0;
    public HeadStatusTypes HeadStatus = HeadStatusTypes.Idle;

    public enum HeadStatusTypes
    {
        Idle = 0,
        PushCharging = 1,
        Pushing = 2,
        Pulling = 3,
    }

    internal float PushChargeForceRatio = 1.0f;

    private void Push()
    {
        HeadStatus = HeadStatusTypes.Pushing;
        HeadCollider.enabled = false;
        Anim.SetTrigger("Push");
        Duck.Wings.Fly();
    }

    private void Pull()
    {
        HeadStatus = HeadStatusTypes.Pulling;
        HeadCollider.enabled = false;
        Anim.SetTrigger("Pull");
        Duck.Wings.Fly();
    }

    void LateUpdate()
    {
        if (!Player.entity.IsControllerOrOwner)
        {
            ExecuteCommand(
                Player.state.Input.LeftTriggerDown,
                Player.state.Input.RightBumperDown,
                Player.state.Input.RightTriggerDown,
                Player.state.Input.RightTriggerUp,
                Player.state.Input.RightTrigger);
        }

        transform.position = Duck.Neck.HeadPosPivot.position;
        if (!Duck.Body.IsPushingNeck && GameManager.Cur_BattleManager.Ball)
        {
            Vector3 diff_BodyToHead = Player.GetPlayerPosition - transform.position;
            Vector3 diff_BallToHead = GameManager.Cur_BattleManager.Ball.transform.position - transform.position;

            float angle = Mathf.Abs(Vector3.SignedAngle(diff_BodyToHead, diff_BallToHead, Vector3.down));

            if (angle > DuckConfig.LookBallAngleThreshold)
            {
                transform.LookAt(GameManager.Cur_BattleManager.Ball.transform.position);
            }
            else
            {
                transform.LookAt(transform.position - diff_BodyToHead.normalized * 10f);
            }
        }
    }
}