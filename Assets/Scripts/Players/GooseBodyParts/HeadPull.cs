using UnityEngine;
using System.Collections;

public class HeadPull : MonoBehaviour
{
    [SerializeField] private Head Head;

    public void OnPull()
    {
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < Head.Goose.PullRadius)
        {
            ko.Kick(Head.ParentPlayerControl.Player.PlayerInfo.RobotIndex, (-diff.normalized) * Head.Goose.PullForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }
}