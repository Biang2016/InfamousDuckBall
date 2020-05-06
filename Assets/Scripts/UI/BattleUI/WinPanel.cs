using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Normal,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
    }

    [SerializeField] private Animator Anim;

    [SerializeField] private Image HeadImage;
    [SerializeField] private Sprite[] TeamHeadSprites;

    [SerializeField] private Image WinText;
    [SerializeField] private Sprite[] TeamTextSingleSprites;
    [SerializeField] private Sprite[] TeamTextMultipleSprites;

    public void Initialize(BattleTypes battleType, TeamNumber winnerTeam)
    {
        HeadImage.sprite = TeamHeadSprites[(int) winnerTeam];

        if (battleType == BattleTypes.Smash)
        {
            WinText.sprite = TeamTextSingleSprites[(int) winnerTeam];
        }

        if (battleType == BattleTypes.FlagRace)
        {
            WinText.sprite = TeamTextMultipleSprites[(int) winnerTeam];
        }
    }

    public void Show()
    {
        Anim.SetTrigger("Show");
    }

    public void OnAnimEnds()
    {
        CloseUIForm();
    }
}