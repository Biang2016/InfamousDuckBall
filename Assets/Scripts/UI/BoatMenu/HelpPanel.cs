using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    [SerializeField] private Image HelpImage;
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Sprite[] HelpImageSprites;

    public void Display()
    {
        SetPage(0);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private int CurrentPage = 0;

    public void OnLeftButtonClick()
    {
        SetPage(CurrentPage - 1);
    }

    public void OnRightButtonClick()
    {
        SetPage(CurrentPage + 1);
    }

    public void OnBackButtonClick()
    {
        BoatMenuManager.Instance.FromHelpBackToStartMenu();
    }

    private void SetPage(int page)
    {
        CurrentPage = page;
        HelpImage.sprite = HelpImageSprites[CurrentPage];
        LeftButton.gameObject.SetActive(CurrentPage > 0);
        RightButton.gameObject.SetActive(CurrentPage < 2);
        BackButton.gameObject.SetActive(CurrentPage == 2);
    }
    
    public void OnButtonHover()
    {
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.Hover, GameManager.Instance.gameObject);
    }

    public void OnButtonClick()
    {
        AudioDuck.Instance.PlaySound(AudioDuck.Instance.Click, GameManager.Instance.gameObject);
    }
}