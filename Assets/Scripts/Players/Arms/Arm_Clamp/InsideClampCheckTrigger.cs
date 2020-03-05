using UnityEngine;
using System.Collections;

public class InsideClampCheckTrigger : MonoBehaviour
{
    [SerializeField] private ArmEnd_Clamp ArmEnd_Clamp;
    public Collider Collider;
    private bool isBallInside = false;
    private bool isPlayerInside = false;
    internal bool IsBallInside = false;
    internal bool IsPlayerInside = false;

    internal Player InsidePlayer;
    internal GoalBall InsideBall;

    void OnTriggerStay(Collider c)
    {
        isBallInside = ArmEnd_Clamp.IsClamped && GameManager.Instance.IsBallLayer(c.gameObject.layer);
        isPlayerInside = ArmEnd_Clamp.IsClamped && GameManager.Instance.IsPlayerColliderLayer(c.gameObject.layer);

        if (isBallInside && !IsBallInside)
        {
            InsideBall = c.GetComponent<GoalBall>();
            int newLayer = GameManager.Instance.Layer_PlayerBall[ArmEnd_Clamp.ParentPlayerControl.Player.PlayerInfo.PlayerNumber];
            if (c.gameObject.layer != newLayer)
            {
                c.gameObject.layer = newLayer;
            }
        }

        if (!isBallInside && IsBallInside)
        {
            if (InsideBall)
            {
                InsideBall.gameObject.layer = GameManager.Instance.Layer_Ball;
                InsideBall = null;
            }
        }

        if (isPlayerInside && !IsPlayerInside)
        {
            InsidePlayer = c.GetComponentInParent<Player>();
        }

        if (!isPlayerInside && IsPlayerInside)
        {
            InsidePlayer = null;
        }

        IsBallInside = isBallInside;
        IsPlayerInside = isPlayerInside;
    }

    void OnTriggerExit(Collider c)
    {
        IsBallInside = GameManager.Instance.IsBallLayer(c.gameObject.layer);
        if (IsBallInside)
        {
            if (InsideBall)
            {
                InsideBall.gameObject.layer = GameManager.Instance.Layer_Ball;
                InsideBall = null;
            }

            IsBallInside = false;
        }

        IsPlayerInside = GameManager.Instance.IsPlayerColliderLayer(c.gameObject.layer);
        if (IsPlayerInside)
        {
            InsidePlayer = null;
            IsPlayerInside = false;
        }
    }
}