using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    [SerializeField] private ArmEnd_SpringHammer ArmEnd_SpringHammer;

    public void OnPull()
    {
        IKickable ko = GameManager.Instance.Cur_BattleManager.Ball;
        Vector3 diff = GameManager.Instance.Cur_BattleManager.Ball.transform.position - transform.position;

        float distance = diff.magnitude;
        if (distance < ArmEnd_SpringHammer.PullRadius)
        {
            ko.Kick(ArmEnd_SpringHammer.ParentPlayerControl.Player.PlayerInfo.RobotIndex, (-diff.normalized) * ArmEnd_SpringHammer.PullForce);
            FXManager.Instance.PlayFX(FX_Type.BallKickParticleSystem, GameManager.Instance.Cur_BattleManager.Ball.transform.position, Quaternion.FromToRotation(Vector3.back, diff.normalized));
        }
    }
}