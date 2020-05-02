using UnityEngine.UI;

public class PasswordPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.PopUp,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Blur);
    }

    public InputField PasswordInputField;
    public Button ConfirmButton;

    public void CancelButtonClick()
    {
        CloseUIForm();
    }
}