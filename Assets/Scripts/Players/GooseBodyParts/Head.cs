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
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.RightTrigger])
        {
            Push();
        }
        else
        {
            if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonDown[ControlButtons.LeftTrigger])
            {
                Pull();
            }
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

    private Vector3 targetLookAtPos = Vector3.zero;

    void LateUpdate()
    {
        transform.position = Goose.Neck.HeadPosPivot.position;

        if (!Goose.Body.IsPushingNeck)
        {
            Vector3 diff_BodyToHead = Goose.Feet.transform.position - transform.position;
            Vector3 diff_BallToHead = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

            float angle = Mathf.Abs(Vector3.SignedAngle(diff_BodyToHead, diff_BallToHead, Vector3.down));

            if (angle > GooseConfig.LookBallAngleThreshold)
            {
                targetLookAtPos = Vector3.Lerp(targetLookAtPos, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Time.deltaTime * GooseConfig.HeadRotateSpeed);
            }
            else
            {
                targetLookAtPos = Vector3.Lerp(targetLookAtPos, transform.position - diff_BodyToHead.normalized * 10f, Time.deltaTime * GooseConfig.HeadRotateSpeed);
            }

            transform.LookAt(targetLookAtPos);
            Debug.DrawRay(transform.position, -diff_BodyToHead.normalized, Color.red, Time.deltaTime);
        }
    }
}