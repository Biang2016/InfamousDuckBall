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
            uiForm_LucencyType: UIFormLucencyTypes.Penetrable);
    }

    void Update()
    {
        if (IsShown && GameManager.Instance.LobbyPanel.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                CloseUIForm();
            }
        }
    }

    [SerializeField] private Animator Anim;
    [SerializeField] private InputField NameInputField;

    public override void Display()
    {
        GameManager.Instance.LobbyPanel.Interactable = false;
        //#if DEBUG
        base.Display();
        Anim.SetTrigger("Show");
        //#else
        //string playerID = PlayerPrefs.GetString("PlayerID");
        //if (!string.IsNullOrWhiteSpace(playerID))
        //{
        //    if (!boatAlreadyMoveIn)
        //    {
        //        boatAlreadyMoveIn = true;
        //        BoatMenuManager.Instance.BoatMoveIn();
        //        CloseUIForm();
        //    }
        //}
        //else
        //{
        //    base.Display();
        //    Anim.SetTrigger("Show");
        //}
        //#endif
    }

    public override void Hide()
    {
        base.Hide();
        GameManager.Instance.LobbyPanel.Interactable = true;
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
            else
            {
                GameManager.Instance.LobbyPanel.UpdateUserName();
            }

            CloseUIForm();
        }
        else
        {
            NoticeManager.Instance.ShowInfoPanelCenter("EVERY DUCK NEEDS A NAME", 0f, 0.5f);
        }
    }
}