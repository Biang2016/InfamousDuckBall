using UnityEngine;
using System.Collections;

public class HeadModel : MonoBehaviour
{
    [SerializeField] private Head Head;

    public void OnPull()
    {
        Head.Goose.Body.PullNeck();
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Head.GooseConfig.PullRadius)
        {
            ko.Kick(Head.ParentPlayerControl.Player.PlayerInfo.RobotIndex, (-diff.normalized) * Head.GooseConfig.PullForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    public void OnPush()
    {
        Head.Goose.Body.PushNeck();
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Head.GooseConfig.PushRadius)
        {
            ko.Kick(Head.ParentPlayerControl.Player.PlayerInfo.RobotIndex, (diff.normalized) * Head.GooseConfig.PushForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }

    public void OnPushOver()
    {
        Head.HeadCollider.enabled = true;
    }

    public void OnPullOver()
    {
        Head.HeadCollider.enabled = true;
    }
}