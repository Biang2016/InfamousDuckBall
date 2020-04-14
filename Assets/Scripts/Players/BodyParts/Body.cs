using System.Collections;
using UnityEngine;

public class Body : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    public Transform BodyRotate;
    public Transform StartPivot;

    internal float SpeedModifier = 1f;
    public float TailSwagDuration = 1f;
    public float BreathRecoverDuration = 1f;

    private Vector3 chargingBackward_mouse = Vector3.zero;

    public void SimulateController()
    {
        if (IsPushingNeck) return;
        Cur_HeadTargetPosition = Duck.Neck.HeadPosPivot.position;
        if (GameManager.Cur_BattleManager.IsStart)
        {
            Cur_HeadTargetPosition.y = GameManager.Cur_BattleManager.Ball.transform.position.y;
        }

        if (Player.Controller != null)
        {
            // Move Neck
            if (Player.Controller.ControllerNumber != PlayerNumber.Player5) // xbox one controller input
            {
                float h = Player.Controller.Axises[ControlAxis.RightStick_H];
                float v = Player.Controller.Axises[ControlAxis.RightStick_V];

                if (!h.Equals(0) || !v.Equals(0) || Duck.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
                {
                    Vector3 neckTargetPos = Duck.Neck.HeadPosPivot.position;

                    if (Duck.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
                    {
                        neckTargetPos += -Duck.Head.transform.forward * DuckConfig.PullChargeBackward;
                        ChargeDistance += DuckConfig.PullChargeBackward;
                    }
                    else
                    {
                        ChargeDistance = 0f;
                    }

                    neckTargetPos += Vector3.forward * (DuckConfig.NeckSpeed * SpeedModifier * h);
                    neckTargetPos += Vector3.right * (DuckConfig.NeckSpeed * SpeedModifier * v);

                    float targetRadius = (neckTargetPos - transform.position).magnitude;
                    if (targetRadius < DuckConfig.Radius * 2)
                    {
                        neckTargetPos = (neckTargetPos - transform.position).normalized * (DuckConfig.Radius * 2) + transform.position;
                    }

                    neckTargetPos.y = GameManager.Cur_BattleManager.Ball.transform.position.y;

                    Vector3 diff = neckTargetPos - Duck.Player.GetPlayerPosition;
                    diff = Vector3.ClampMagnitude(diff, DuckConfig.MaxNeckLength);
                    diff = diff.magnitude < DuckConfig.MinNeckLength ? diff.normalized * DuckConfig.MinNeckLength : diff;
                    neckTargetPos = diff + Duck.Player.GetPlayerPosition;
                    MoveNeckTo(neckTargetPos);
                }
            }
            else // if keyboard input
            {
                Vector3 neckTargetPos = Duck.Neck.HeadPosPivot.position;

                if (Duck.Head.HeadStatus == Head.HeadStatusTypes.PushCharging)
                {
                    chargingBackward_mouse += Duck.Head.transform.forward * DuckConfig.PullChargeBackward;
                    ChargeDistance += DuckConfig.PullChargeBackward;
                }
                else
                {
                    chargingBackward_mouse = Vector3.zero;
                    ChargeDistance = 0f;
                }

                Ray ray = GameManager.Cur_BattleManager.BattleCamera.ScreenPointToRay(Input.mousePosition);
                if (GameManager.Cur_BattleManager.FloorPlane.Raycast(ray, out float enter))
                {
                    Vector3 mousePos = ray.GetPoint(enter);
                    Vector3 dir = (mousePos - (neckTargetPos + chargingBackward_mouse)).normalized;

                    neckTargetPos += dir * (DuckConfig.NeckSpeed * SpeedModifier);

                    float targetRadius = (neckTargetPos - transform.position).magnitude;
                    if (targetRadius < DuckConfig.Radius * 2)
                    {
                        neckTargetPos = (neckTargetPos - transform.position).normalized * (DuckConfig.Radius * 2) + transform.position;
                    }

                    neckTargetPos.y = GameManager.Cur_BattleManager.Ball.transform.position.y;

                    Vector3 diff = neckTargetPos - Duck.Player.GetPlayerPosition;
                    diff = Vector3.ClampMagnitude(diff, DuckConfig.MaxNeckLength);
                    diff = diff.magnitude < DuckConfig.MinNeckLength ? diff.normalized * DuckConfig.MinNeckLength : diff;
                    neckTargetPos = diff + Duck.Player.GetPlayerPosition;
                    neckTargetPos -= chargingBackward_mouse / 10f;
                    MoveNeckTo(neckTargetPos);
                }
            }
        }
    }

    internal float ChargeDistance = 0f;

    public void PullNeck()
    {
        if (!IsPushingNeck)
        {
            StartCoroutine(Co_PullNeck(Duck.Head.transform.forward));
        }
    }

    public void PushNeck()
    {
        if (!IsPushingNeck)
        {
            StartCoroutine(Co_PushNeck(Duck.Head.transform.forward, ChargeDistance));
        }
    }

    internal bool IsPushingNeck = false;

    IEnumerator Co_PullNeck(Vector3 dir)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Duck.Neck.HeadPosPivot.position;

        float dist = (Duck.Head.transform.position + Duck.Head.transform.forward * DuckConfig.PullBallStopFromHead - GameManager.Cur_BattleManager.Ball.transform.position).magnitude;
        dist = Mathf.Min(DuckConfig.PullNeckDistance, dist);

        for (int i = 0; i < DuckConfig.PullNeckFrame; i++)
        {
            neckTargetPos += dir * dist / DuckConfig.PullNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        for (int i = 0; i < DuckConfig.PullNeckFrame; i++)
        {
            neckTargetPos -= dir * dist / DuckConfig.PullNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        IsPushingNeck = false;
    }

    IEnumerator Co_PushNeck(Vector3 dir, float chargeDistance)
    {
        IsPushingNeck = true;
        Vector3 neckTargetPos = Duck.Neck.HeadPosPivot.position;

        float dist = (Duck.GetHeadPosition - GameManager.Cur_BattleManager.Ball.transform.position).magnitude;
        float dist_forward = Mathf.Min(DuckConfig.PushNeckDistance + chargeDistance, dist);
        float dist_backward = Mathf.Min(DuckConfig.PushNeckDistance, dist);

        for (int i = 0; i < DuckConfig.PushNeckFrame; i++)
        {
            neckTargetPos += dir * dist_forward / DuckConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        for (int i = 0; i < DuckConfig.PushNeckFrame; i++)
        {
            neckTargetPos -= dir * dist_backward / DuckConfig.PushNeckFrame;
            MoveNeckTo(neckTargetPos);
            yield return null;
        }

        IsPushingNeck = false;
        ChargeDistance = 0f;
    }

    private Quaternion BodyTargetRot;

    internal Vector3 Cur_HeadTargetPosition;

    public void MoveNeckTo(Vector3 targetPos)
    {
        Vector3 diff = Vector3.Scale(targetPos - StartPivot.position, new Vector3(1, 0, 1));
        float angleOffset = Vector3.SignedAngle(transform.forward, diff, Vector3.up);
        BodyTargetRot = Quaternion.Euler(new Vector3(0, angleOffset, 0));
        BodyRotate.rotation = BodyTargetRot;
        Duck.Neck.NeckTargetPos = targetPos;
        Duck.Neck.NeckDeform();
        if (Player.entity.HasControl)
        {
            Cur_HeadTargetPosition = targetPos;
        }
    }

    protected void LateUpdate()
    {
        if (!Player.entity.HasControl)
        {
            MoveNeckTo(Player.state.HeadTargetPosition);
        }
        else
        {
            Duck.Neck.NeckTargetPos = Cur_HeadTargetPosition;
            Duck.Neck.NeckDeform();
        }

        transform.position = Duck.Player.GetPlayerPosition;
        float tail = BodyAnimator.GetFloat("Tail");
        tail = Mathf.Clamp(tail - 0.1f / (TailSwagDuration * Application.targetFrameRate), 0, 1);
        if (tail < 0.9f)
        {
            tail = 0;
        }

        BodyAnimator.SetFloat("Tail", tail);

        float breath = BodyAnimator.GetFloat("Breath");
        breath = Mathf.Clamp(breath + 0.1f / (BreathRecoverDuration * Application.targetFrameRate), 0.0f, 0.1f);
        if (breath >= 0.1f)
        {
            breath = 0.35f;
        }

        BodyAnimator.SetFloat("Breath", breath);

        float gasp = BodyAnimator.GetFloat("Gasp");
        gasp = Mathf.Clamp(gasp - 0.1f / (BreathRecoverDuration * Application.targetFrameRate), 0, 1);
        if (gasp < 0.9f)
        {
            gasp = 0;
        }

        BodyAnimator.SetFloat("Gasp", gasp);
    }

    public Animator BodyAnimator;
}