using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour, IPlayerControl
{
    private PlayerControl ParentPlayerControl;

    public void Initialize(PlayerControl parentPlayerControl)
    {
        ParentPlayerControl = parentPlayerControl;
    }

    [SerializeField] private float MoveSpeed = 0.3f;
    [SerializeField] private JoystickAxis HorizontalAxis = JoystickAxis.None;
    [SerializeField] private JoystickAxis VerticalAxis = JoystickAxis.None;

    void FixedUpdate()
    {
        if (ParentPlayerControl && ParentPlayerControl.Controllable)
        {
            Move();
        }
    }

    private void Move()
    {
        float hor = Input.GetAxis(HorizontalAxis + "_" + ParentPlayerControl.Player.PlayerNumber);
        Vector3 tarPos = Vector3.zero;
        if (Mathf.Abs(hor) > 0.3f)
        {
            tarPos += Vector3.forward * MoveSpeed * hor;
        }

        float ver = Input.GetAxis(VerticalAxis + "_" + ParentPlayerControl.Player.PlayerNumber);
        if (Mathf.Abs(ver) > 0.3f)
        {
            tarPos += Vector3.right * MoveSpeed * ver;
        }

        Vector3 tarPosGlobal = transform.TransformPoint(tarPos);

        //tarPosGlobal.x = Mathf.Clamp(tarPosGlobal.x, GameManager.Instance.Cur_BattleManager.X_Min, GameManager.Instance.Cur_BattleManager.X_Max);
        //tarPosGlobal.z = Mathf.Clamp(tarPosGlobal.z, GameManager.Instance.Cur_BattleManager.Z_Min, GameManager.Instance.Cur_BattleManager.Z_Max);

        Vector3 dest = ParentPlayerControl.Player.TryToMove(tarPosGlobal, GameManager.Instance.PlayerRadius);
        tarPosGlobal = dest;

        transform.position = tarPosGlobal;
    }
}