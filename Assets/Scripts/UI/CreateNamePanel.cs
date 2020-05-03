using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNamePanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Normal,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.ImPenetrable);
    }

    [SerializeField] private Animator Anim;
    [SerializeField] private InputField NameInputField;

    public override void Display()
    {
#if DEBUG
        base.Display();
        Anim.SetTrigger("Show");
#else
        string playerID = PlayerPrefs.GetString("PlayerID");
        if (!string.IsNullOrWhiteSpace(playerID))
        {
            if (!boatAlreadyMoveIn)
            {
                boatAlreadyMoveIn = true;
                BoatMenuManager.Instance.BoatMoveIn();
            }
        }
        else
        {
            base.Display();
            Anim.SetTrigger("Show");
        }
#endif
    }

    private bool boatAlreadyMoveIn = false;

    public void OnConfirmButtonClick()
    {
        if (!string.IsNullOrWhiteSpace(NameInputField.text))
        {
            PlayerPrefs.SetString("PlayerID", NameInputField.text);
            Anim.SetTrigger("Hide");
            if (!boatAlreadyMoveIn)
            {
                boatAlreadyMoveIn = true;
                BoatMenuManager.Instance.BoatMoveIn();
            }
        }
        else
        {
            NoticeManager.Instance.ShowInfoPanelCenter("EVERY DUCK NEEDS A NAME", 0f, 0.5f);
        }

        
    }
}