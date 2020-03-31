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
        if (controllerIndex != PlayerNumber.Player5) // xbox one controller input
        {
            float h = MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_H];
            float v = MultiControllerManager.Instance.Controllers[controllerIndex].Axises[ControlAxis.RightStick_V];

            if (!h.Equals(0) || !v.Equals(0) || Goose.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
            {
                Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

                if (Goose.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
                {
                    neckTargetPos += -Goose.Head.transform.forward * 0.01f;
                    ChargeDistance += 0.01f;
                }
                else
                {
                    ChargeDistance = 0f;
                }

                neckTargetPos += Vector3.forward * (GooseConfig.NeckSpeed * SpeedModifier * h);
                neckTargetPos += Vector3.right * (GooseConfig.NeckSpeed * SpeedModifier * v);

                float targetRadius = (neckTargetPos - transform.position).magnitude;
                if (targetRadius < GooseConfig.Radius * 2)
                {
                    neckTargetPos = (neckTargetPos - transform.position).normalized * (GooseConfig.Radius * 2) + transform.position;
                }

                neckTargetPos.y = GameManager.Instance.GetBallPosition().y;

                Vector3 diff = neckTargetPos - ParentPlayerControl.Player.GetPlayerPosition;
                diff = diff.magnitude > GooseConfig.MaxNeckLength ? diff.normalized * GooseConfig.MaxNeckLength : diff;
                neckTargetPos = diff + ParentPlayerControl.Player.GetPlayerPosition;
                MoveNeckTo(neckTargetPos);
            }
        }
        else // if keyboard input
        {
            Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

            if (Goose.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
            {
                neckTargetPos += -Goose.Head.transform.forward * 0.01f;
                ChargeDistance += 0.01f;
            }
            else
            {
                ChargeDistance = 0f;
            }

            Ray ray = GameManager.Instance.GetCamera().ScreenPointToRay(Input.mousePosition);
            if (GameManager.Instance.FloorPlane.Raycast(ray, out float enter))
            {
                Vector3 mousePos = ray.GetPoint(enter);
                Vector3 dir = (mousePos - neckTargetPos).normalized;

                neckTargetPos += dir * (GooseConfig.NeckSpeed * SpeedModifier);

                float targetRadius = (neckTargetPos - transform.position).magnitude;
                if (targetRadius < GooseConfig.Radius * 2)
                {
                    neckTargetPos = (neckTargetPos - transform.position).normalized * (GooseConfig.Radius * 2) + transform.position;
                }

                neckTargetPos.y = GameManager.Instance.GetBallPosition().y;

                Vector3 diff = neckTargetPos - ParentPlayerControl.Player.GetPlayerPosition;
                diff = diff.magnitude > GooseConfig.MaxNeckLength ? diff.normalized * GooseConfig.MaxNeckLength : diff;
                neckTargetPos = diff + ParentPlayerControl.Player.GetPlayerPosition;
                MoveNeckTo(neckTargetPos);
            }
        }
    }

    internal float ChargeDistance = 0f;

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
            StartCoroutine(Co_PushNeck(Goose.Head.transform.forward, ChargeDistance));
        }
    }

    internal bool IsPushingNeck = false;

    IEnumerator Co_PullNeck(Vector3 dir)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

        float dist = (Goose.Head.transform.position + Goose.Head.transform.forward * GooseConfig.PullBallStopFromHead - GameManager.Instance.GetBallPosition()).magnitude;
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

    IEnumerator Co_PushNeck(Vector3 dir, float chargeDistance)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Goose.Neck.HeadPosPivot.position;

        float dist = (Goose.GetHeadPosition - GameManager.Instance.GetBallPosition()).magnitude;
        float dist_forward = Mathf.Min(GooseConfig.PushNeckDistance + chargeDistance, dist);
        float dist_backward = Mathf.Min(GooseConfig.PushNeckDistance, dist);

        for (int i = 0; i < GooseConfig.PushNeckFrame; i++)
        {
            neckTargetPos += dir * dist_forward / GooseConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        for (int i = 0; i < GooseConfig.PushNeckFrame; i++)
        {
            neckTargetPos -= dir * dist_backward / GooseConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        IsPushingNeck = false;
        ChargeDistance = 0f;
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
        transform.position = ParentPlayerControl.Player.GetPlayerPosition;
    }
}