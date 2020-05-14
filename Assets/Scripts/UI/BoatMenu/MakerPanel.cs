using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakerPanel : BaseUIForm
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

    [SerializeField] private Image SunglassesImage;
    [SerializeField] private Sprite[] SunglassesSprites;

    public override void Display()
    {
        base.Display();
        SunglassesImage.sprite = SunglassesSprites[Random.Range(0, SunglassesSprites.Length)];
    }
}