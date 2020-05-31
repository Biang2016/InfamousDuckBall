using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogoPanel : BaseUIForm
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

        ClientVersionText.text = Application.version;
    }

    void Update()
    {
    }

    [SerializeField] private Animator GameLogoAnim;
    [SerializeField] private Text ClientVersionText;

    public void GameLogoDrop()
    {
        GameLogoAnim.SetTrigger("Show");
        GameLogoAnim.ResetTrigger("Hide");
    }

    public void GameLogoPullUp()
    {
        GameLogoAnim.SetTrigger("Hide");
        GameLogoAnim.ResetTrigger("Show");
    }
}