using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuPanel_Boat : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button HelpButton;
    [SerializeField] private Button CreditButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button BackButton;

    public void OnPlayButtonClick()
    {
        BoatMenuManager.Instance.FromStartMenuToLobby();
    }

    public void OnHelpButtonClick()
    {
        BoatMenuManager.Instance.FromStartMenuToHelp();
    }

    public void OnCreditButtonClick()
    {
        BoatMenuManager.Instance.BoatMenuBoat.ScoreRingsExplode();
        PlayButton.gameObject.SetActive(false);
        HelpButton.gameObject.SetActive(false);
        CreditButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        GameManager.Instance.GameLogoPanel.GameLogoDrop();
        BoatMenuManager.Instance.BoatMenuBoat.ScoreRingRecover();
        PlayButton.gameObject.SetActive(true);
        HelpButton.gameObject.SetActive(true);
        CreditButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
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