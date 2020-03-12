using UnityEngine;

public class Head : GooseBodyPart
{
    public Collider HeadCollider;

    public override void Initialize(PlayerControl parentPlayerControl)
    {
        base.Initialize(parentPlayerControl);
        foreach (Collider c in transform.GetComponentsInChildren<Collider>())
        {
            if (c.gameObject.layer == GameManager.Instance.Layer_BallKicker)
            {
                string layerName = "BallKicker" + ((int) (ParentPlayerControl.Player.PlayerInfo.PlayerNumber) + 1);
                int layer = LayerMask.NameToLayer(layerName);
                c.gameObject.layer = layer;
            }
        }
    }

    public Animator Anim;

    protected override void Operate_AI()
    {
    }

    private float PushChargeTimeTick = 0;
    public HeadStatusTypes HeadStatus = HeadStatusTypes.Idle;

    public enum HeadStatusTypes
    {
        Idle = 0,
        PushCharging = 1,
        Pushing = 2,
        Pulling = 3,
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        bool rightBumperDown = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightBumper];
        bool rightDown = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger];
        bool rightUp = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.RightTrigger];
        bool rightPressed = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonPressed[ControlButtons.RightTrigger];
        bool leftDown = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.LeftTrigger];
        
        switch (HeadStatus)
        {
            case HeadStatusTypes.Idle:
            {
                if (leftDown)
                {
                    Pull();
                }
                else if (rightDown)
                {
                    HeadStatus = HeadStatusTypes.PushCharging;
                }else if (rightPressed)
                    {
                        HeadStatus = HeadStatusTypes.PushCharging;

                    }
                    else if(rightBumperDown)
                    {
                        PushChargeForceRatio = 0f;
                        Push();
                    }

                    break;
            }
            case HeadStatusTypes.PushCharging:
            {
                if (rightUp)
                {
                        PushChargeForceRatio = PushChargeTimeTick / GooseConfig.PushChargeTimeMaxDuration;
                        PushChargeTimeTick = 0;
                        Push();
                }
                else if (rightPressed)
                {
                    PushChargeTimeTick += Time.deltaTime;
                    if (PushChargeTimeTick > GooseConfig.PushChargeTimeMaxDuration)
                    {
                        PushChargeForceRatio = PushChargeTimeTick / GooseConfig.PushChargeTimeMaxDuration;
                            PushChargeTimeTick = 0;
                        Push();
                    }
                }

                break;
            }
        }
    }

    internal float PushChargeForceRatio = 1.0f;


    private void Push()
    {
        HeadStatus = HeadStatusTypes.Pushing;
        HeadCollider.enabled = false;
        Anim.SetTrigger("Push");
        Goose.Wings.Fly();
    }

    private void Pull()
    {
        HeadStatus = HeadStatusTypes.Pulling;
        HeadCollider.enabled = false;
        Anim.SetTrigger("Pull");
        Goose.Wings.Fly();
    }

    void LateUpdate()
    {
        float diff = (transform.position - Goose.Neck.HeadPosPivot.position).magnitude;
        if (diff > 0.5f)
        {
            //Debug.Log(diff);
            //Debug.LogError("st");
        }

        transform.position = Goose.Neck.HeadPosPivot.position;

        if (!Goose.Body.IsPushingNeck)
        {
            Vector3 diff_BodyToHead = ParentPlayerControl.Player.GetPlayerPosition - transform.position;
            Vector3 diff_BallToHead = GameManager.Instance.GetBallPosition() - transform.position;

            float angle = Mathf.Abs(Vector3.SignedAngle(diff_BodyToHead, diff_BallToHead, Vector3.down));

            if (angle > GooseConfig.LookBallAngleThreshold)
            {
                transform.LookAt(GameManager.Instance.GetBallPosition());
            }
            else
            {
                transform.LookAt(transform.position - diff_BodyToHead.normalized * 10f);
            }
        }
    }
}