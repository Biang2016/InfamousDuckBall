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

    public void SetScore(int player1, int player2)
    {
        Score1Text.text = player1.ToString();
        Score2Text.text = player2.ToString();
    }

    public void OnReset()
    {
        GameManager.Instance.ResetBall();
    }
}