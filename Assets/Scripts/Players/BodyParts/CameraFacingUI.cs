using UnityEngine;

public class CameraFacingUI : MonoBehaviour
{
    void LateUpdate()
    {
        if (GameManager.Instance.Cur_BattleManager)
        {
            transform.LookAt(GameManager.Instance.Cur_BattleManager.BattleCamera.transform);
        }
    }
}