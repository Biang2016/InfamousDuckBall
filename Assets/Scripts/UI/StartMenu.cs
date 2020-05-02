using UnityEngine;
using System.Collections;

public class StartMenu : BaseUIForm
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
    }

    [SerializeField] private Animator GameLogoAnim;

    public void GameLogoDrop()
    {
        GameLogoAnim.SetTrigger("Show");
    }

    public void GameLogoPullUp()
    {
        GameLogoAnim.SetTrigger("Hide");
    }
}