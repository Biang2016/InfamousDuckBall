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

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        bool right = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger];
        bool left = MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.LeftTrigger];

        if (!left && right)
        {
            Push();
        }

        if (left && !right)
        {
            Pull();
        }
    }

    private void Push()
    {
        HeadCollider.enabled = false;
        Anim.SetTrigger("Push");
        Goose.Wings.Fly();
    }

    private void Pull()
    {
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