using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoundPanel : BaseUIForm
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

    [SerializeField] private Sprite[] RoundSprites;
    [SerializeField] private Sprite StartSprite;
    [SerializeField] private Sprite[] CountingSprites;

    [SerializeField] private Image BannerTextImage;
    [SerializeField] private Animator BannerAnim;
    [SerializeField] private Animator BannerTextAnim;

    private Coroutine showCoroutine;

    public override void Display()
    {
        base.Display();
        if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
        {
            PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = false;
        }
    }

    public void Show(int round)
    {
        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
            // AudioDuck.Instance.StopSpecial(AudioDuck.Instance.count321);
        }

        showCoroutine = StartCoroutine(Co_Show(round));
    }

    IEnumerator Co_Show(int round)
    {
        if (round != -1)
        {
            BannerTextImage.sprite = RoundSprites[round - 1];
        }
        else
        {
            BannerTextImage.sprite = CountingSprites[2];
        }

        BannerAnim.SetTrigger("Show");
        yield return new WaitForSeconds(0.2f);

        if (round != -1)
        {
            BannerTextAnim.SetTrigger("Jump");
            yield return new WaitForSeconds(1f);
        }

        // AudioDuck.Instance.PlaySpecial(AudioDuck.Instance.count321);

        BannerTextImage.sprite = CountingSprites[2];
        BannerTextAnim.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);

        BannerTextImage.sprite = CountingSprites[1];
        BannerTextAnim.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);

        BannerTextImage.sprite = CountingSprites[0];
        BannerTextAnim.SetTrigger("Jump");
        yield return new WaitForSeconds(1f);

        BannerTextImage.sprite = StartSprite;
        BannerTextAnim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);

        BannerAnim.SetTrigger("Hide");
        if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
        {
            PlayerObjectRegistry.MyPlayer.PlayerController.Controller.Active = true;
        }

        CloseUIForm();
        showCoroutine = null;
    }

    public override void Hide()
    {
        base.Hide();
        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
            // AudioDuck.Instance.StopSpecial(AudioDuck.Instance.count321);
        }
    }
}