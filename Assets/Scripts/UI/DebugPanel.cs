using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : BaseUIForm
{
    [SerializeField] private Text Score1Text;
    [SerializeField] private Text ScoreDotText;
    [SerializeField] private Text Score2Text;
    [SerializeField] private Text Score3Text;
    [SerializeField] private Text Score4Text;

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

    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void RefreshScore()
    {
        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player1))
        {
            Score1Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player1].Score.ToString();
        }
        else
        {
            Score1Text.text = "-";
        }

        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player2))
        {
            Score2Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player2].Score.ToString();
        }
        else
        {
            Score2Text.text = "-";
        }

        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player3))
        {
            Score3Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player3].Score.ToString();
        }
        else
        {
            Score3Text.text = "-";
        }

        if (GameManager.Instance.PlayerDict.ContainsKey(PlayerNumber.Player4))
        {
            Score4Text.text = GameManager.Instance.PlayerDict[PlayerNumber.Player4].Score.ToString();
        }
        else
        {
            Score4Text.text = "-";
        }
    }

    public void SetScoreShown(bool shown)
    {
        Score1Text.enabled = shown;
        Score2Text.enabled = shown;
        ScoreDotText.enabled = shown;
    }

    public void OnReset()
    {
        GameManager.Instance.Cur_BattleManager.ResetBall();
    }
}