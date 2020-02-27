using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : BaseUIForm
{
    [SerializeField] private Text Score1Text;
    [SerializeField] private Text Score2Text;

    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Fixed,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
    }

    public void RefreshScore()
    {
        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player1))
        {
            Score1Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player1].ToString();
        }

        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player2))
        {
            Score2Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player2].ToString();
        }
    }

    public void OnReset()
    {
        GameManager.Instance.Cur_BattleManager.ResetBall();
    }
}