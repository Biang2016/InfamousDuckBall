using System.Collections;
using UnityEngine;

public class Body : GooseBodyPart
{
    public Transform BodyRotate;
    public Transform StartPivot;

    internal float SpeedModifier = 1f;

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (IsPushingNeck) return;

        // Move Neck
        {
            Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

            neckTargetPos += Vector3.forward * GooseConfig.NeckSpeed * SpeedModifier * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_H];
            neckTargetPos += Vector3.right * GooseConfig.NeckSpeed * SpeedModifier * MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_V];

            float targetRadius = (neckTargetPos - transform.position).magnitude;
            if (targetRadius < GooseConfig.Radius * 2)
            {
                neckTargetPos = (neckTargetPos - transform.position).normalized * GooseConfig.Radius * 2 + transform.position;
            }

            neckTargetPos.y = GameManager.Instance.Cur_BattleManager.Ball.transform.position.y;

            Vector3 diff = neckTargetPos - Goose.Feet.transform.position;
            diff = diff.magnitude > GooseConfig.MaxNeckLength ? diff.normalized * GooseConfig.MaxNeckLength : diff;
            neckTargetPos = diff + Goose.Feet.transform.position;
            MoveNeckTo(neckTargetPos);
        }
    }

    public void PullNeck()
    {
        if (!IsPushingNeck)
        {
            StartCoroutine(Co_PullNeck(Goose.Head.transform.forward));
        }
    }

    public void PushNeck()
    {
        if (!IsPushingNeck)
        {
            StartCoroutine(Co_PushNeck(Goose.Head.transform.forward));
        }
    }

    internal bool IsPushingNeck = false;

    IEnumerator Co_PullNeck(Vector3 dir)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

        float dist = (Goose.Head.transform.position - GameManager.Instance.Cur_BattleManager.Ball.transform.position).magnitude;
        dist = Mathf.Min(GooseConfig.PullNeckDistance, dist);

        for (int i = 0; i < GooseConfig.PullNeckFrame; i++)
        {
            neckTargetPos += dir * dist / GooseConfig.PullNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        for (int i = 0; i < GooseConfig.PullNeckFrame; i++)
        {
            neckTargetPos -= dir * dist / GooseConfig.PullNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        IsPushingNeck = false;
    }

    IEnumerator Co_PushNeck(Vector3 dir)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

        float dist = (Goose.Head.transform.position - GameManager.Instance.Cur_BattleManager.Ball.transform.position).magnitude;
        dist = Mathf.Min(GooseConfig.PushNeckDistance, dist);

        for (int i = 0; i < GooseConfig.PushNeckFrame; i++)
        {
            neckTargetPos += dir * dist / GooseConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        for (int i = 0; i < GooseConfig.PushNeckFrame; i++)
        {
            neckTargetPos -= dir * dist / GooseConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        IsPushingNeck = false;
    }

    protected override void Operate_AI()
    {
    }

    private Quaternion BodyTargetRot;

    private void MoveNeckTo(Vector3 targetPos)
    {
        Vector3 diff = Vector3.Scale(targetPos - StartPivot.position, new Vector3(1, 0, 1));
        float angleOffset = Vector3.SignedAngle(transform.forward, diff, Vector3.up);
        BodyTargetRot = Quaternion.Euler(new Vector3(0, angleOffset, 0));
        BodyRotate.rotation = BodyTargetRot;
        Goose.Neck.MoveNeckTo(targetPos);
    }

    protected void LateUpdate()
    {
        transform.position = Goose.Feet.transform.position;
        Goose.Head.transform.position = Goose.Neck.HeadPosPivot.position;
    }
}