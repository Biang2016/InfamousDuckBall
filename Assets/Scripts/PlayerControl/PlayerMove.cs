using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour, IPlayerControl
{
    private PlayerNumber PlayerNumber;

    public void SetPlayerNumber(PlayerNumber playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    [SerializeField] private float MoveSpeed = 0.3f;
    [SerializeField] private JoystickAxis HorizontalAxis = JoystickAxis.None;
    [SerializeField] private JoystickAxis VerticalAxis = JoystickAxis.None;

    private void Move()
    {
        float hor = Input.GetAxis(HorizontalAxis + "_" + PlayerNumber);
        Vector3 tarPos = Vector3.zero;
        if (Mathf.Abs(hor) > 0.3f)
        {
            tarPos += Vector3.forward * MoveSpeed * hor;
        }

        float ver = Input.GetAxis(VerticalAxis + "_" + PlayerNumber);
        if (Mathf.Abs(ver) > 0.3f)
        {
            tarPos += Vector3.right * MoveSpeed * ver;
        }

        Vector3 tarPosGlobal = transform.TransformPoint(tarPos);
        tarPosGlobal.x = Mathf.Clamp(tarPosGlobal.x, GameManager.Instance.X_Min, GameManager.Instance.X_Max);
        tarPosGlobal.z = Mathf.Clamp(tarPosGlobal.z, GameManager.Instance.Z_Min, GameManager.Instance.Z_Max);

        Player enemyPlayer = null;
        switch (PlayerNumber)
        {
            case PlayerNumber.P1:
            {
                enemyPlayer = GameManager.Instance.Player2;
                break;
            }
            case PlayerNumber.P2:
            {
                enemyPlayer = GameManager.Instance.Player1;
                break;
            }
        }

        Vector3 diff = Vector3.Scale(new Vector3(1, 0, 1), tarPosGlobal - enemyPlayer.transform.position);
        float distance = diff.magnitude;
        if (distance < 2 * GameManager.Instance.PlayerRadius)
        {
            Vector3 offset = diff.normalized * 2 * GameManager.Instance.PlayerRadius;
            tarPosGlobal = enemyPlayer.transform.position + offset;
        }

        transform.position = tarPosGlobal;
    }

    void FixedUpdate()
    {
        Move();
    }
}