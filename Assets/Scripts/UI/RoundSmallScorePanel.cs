using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundSmallScorePanel : BaseUIForm
{
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

    [SerializeField] private RectTransform Panel;
    [SerializeField] private Text RoomStatusText;
    [SerializeField] private Image Team1Ring;
    [SerializeField] private Image Team2Ring;
    [SerializeField] private Text Team1ScoreText;
    [SerializeField] private Text Team2ScoreText;
    [SerializeField] private Animator Team1Anim;
    [SerializeField] private Animator Team2Anim;

    public void SetScoreShown(bool shown)
    {
        Team1Ring.enabled = shown;
        Team2Ring.enabled = shown;
        Team1ScoreText.enabled = shown;
        Team2ScoreText.enabled = shown;
        RoomStatusText.enabled = !shown;
    }

    public void SetRoomStatusText(string text)
    {
        RoomStatusText.text = text;
    }

    private bool CurrentPosUp;

    public void SetPanelPos(bool up)
    {
        CurrentPosUp = up;
        if (up)
        {
            Panel.anchorMax = new Vector2(Panel.anchorMax.x, 1);
            Panel.anchorMin = new Vector2(Panel.anchorMin.x, 1);
            Panel.pivot = new Vector2(Panel.pivot.x, 1);
        }
        else
        {
            Panel.anchorMax = new Vector2(Panel.anchorMax.x, 0);
            Panel.anchorMin = new Vector2(Panel.anchorMin.x, 0);
            Panel.pivot = new Vector2(Panel.pivot.x, 0);
        }
    }

    public void RefreshScore_Team1(int team1Score)
    {
        string team1 = CurrentPosUp ? ("*" + team1Score) : (team1Score + "/10");

        if (CurrentPosUp)
        {
            if (team1Score == 5)
            {
                Team1Ring.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
                Team1ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
            }
            else
            {
                Team1Ring.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
                Team1ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
            }
        }
        else
        {
            if (team1Score == 0)
            {
                Team1Ring.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
                Team1ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
            }
            else
            {
                Team1Ring.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
                Team1ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
            }
        }

        if (!Team1ScoreText.text.Equals(team1))
        {
            Team1ScoreText.text = team1;
            Team1Anim.SetTrigger("Jump");
        }
    }

    public void RefreshScore_Team2(int team2Score)
    {
        string team2 = CurrentPosUp ? ("*" + team2Score) : (team2Score + "/10");

        if (CurrentPosUp)
        {
            if (team2Score == 5)
            {
                Team2Ring.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
                Team2ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
            }
            else
            {
                Team2Ring.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
                Team2ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
            }
        }
        else
        {
            if (team2Score == 0)
            {
                Team2Ring.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
                Team2ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFF3D");
            }
            else
            {
                Team2Ring.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
                Team2ScoreText.color = ClientUtils.HTMLColorToColor("#FFFFFFFF");
            }
        }

        if (!Team2ScoreText.text.Equals(team2))
        {
            Team2ScoreText.text = team2;
            Team2Anim.SetTrigger("Jump");
        }
    }
}