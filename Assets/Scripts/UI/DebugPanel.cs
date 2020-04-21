using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : BaseUIForm
{
    [SerializeField] private Text Score1Text;
    [SerializeField] private Text Score2Text;
    [SerializeField] private Text Score3Text;
    [SerializeField] private Text Score4Text;
    [SerializeField] private Text Score2Dot;
    [SerializeField] private Text Score3Dot;
    [SerializeField] private Text Score4Dot;

    [SerializeField] private Text LevelNameText;
    [SerializeField] private Text StartTipText;

    public ConfigRows ConfigRows;

    private SortedDictionary<TeamNumber, Text> TeamScoreTextDict = new SortedDictionary<TeamNumber, Text>();
    private SortedDictionary<TeamNumber, Text> TeamScoreDotDict = new SortedDictionary<TeamNumber, Text>();

    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Fixed,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);

        TeamScoreTextDict.Add(TeamNumber.Team1, Score1Text);
        TeamScoreTextDict.Add(TeamNumber.Team2, Score2Text);
        TeamScoreTextDict.Add(TeamNumber.Team3, Score3Text);
        TeamScoreTextDict.Add(TeamNumber.Team4, Score4Text);
        TeamScoreDotDict.Add(TeamNumber.Team2, Score2Dot);
        TeamScoreDotDict.Add(TeamNumber.Team3, Score3Dot);
        TeamScoreDotDict.Add(TeamNumber.Team4, Score4Dot);
    }

    public Text fpsText;
    private float deltaTime;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void RefreshScore(bool useMegaScore)
    {
        foreach (KeyValuePair<TeamNumber, Team> kv in GameManager.Instance.Cur_BattleManager.TeamDict)
        {
            if (kv.Value.TeamPlayers.Count != 0)
            {
                if (useMegaScore)
                {
                    TeamScoreTextDict[kv.Key].text = kv.Value.MegaScore.ToString();
                }
                else
                {
                    TeamScoreTextDict[kv.Key].text = kv.Value.Score.ToString();
                }
            }

            TeamScoreTextDict[kv.Key].gameObject.SetActive(kv.Value.TeamPlayers.Count != 0);
            if (kv.Key != TeamNumber.Team1) TeamScoreDotDict[kv.Key].gameObject.SetActive(kv.Value.TeamPlayers.Count != 0);
        }
    }

    public void RefreshLevelName()
    {
        LevelNameText.text = GameManager.Instance.Cur_BattleManager.BattleType.ToString();
    }

    public void SetStartTipShown(bool shown,string text)
    {
        StartTipText.enabled = shown;
        StartTipText.text = text;
    }

    [SerializeField] private Text WinText;
    [SerializeField] private Animator WinTextAnim;

    public void Wins(TeamNumber teamNumber)
    {
        switch (teamNumber)
        {
            case TeamNumber.Team1:
            {
                WinText.text = "RED WINS";
                WinText.color = Color.red;
                WinTextAnim.SetTrigger("Show");
                StartCoroutine(Co_HideWinText());
                break;
            }
            case TeamNumber.Team2:
            {
                WinText.text = "BLUE WINS";
                WinText.color = Color.blue;
                WinTextAnim.SetTrigger("Show");
                StartCoroutine(Co_HideWinText());
                break;
            }
        }
    }

    IEnumerator Co_HideWinText()
    {
        yield return new WaitForSeconds(3f);
        WinTextAnim.SetTrigger("Hide");
    }
}