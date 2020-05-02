using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoatMenu : MonoBehaviour
{
    [SerializeField] private WholeBoat_BoatMenu WholeBoat_BoatMenu;
    [SerializeField] private Animator Anim;

    public void BoatMoveIn()
    {
        Anim.SetTrigger("MoveIn");
    }

    [SerializeField] private Button PlayButton;
    [SerializeField] private Button HelpButton;
    [SerializeField] private Button CreditButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button BackButton;

    public void OnPlayButtonClick()
    {
        WholeBoat_BoatMenu.RingsExplode();
        UIManager.Instance.ShowUIForms<LobbyPanel>();
    }

    public void OnHelpButtonClick()
    {
        WholeBoat_BoatMenu.RingsExplode();
        //UIManager.Instance.ShowUIForms<LobbyPanel>();
    }

    public void OnCreditButtonClick()
    {
        WholeBoat_BoatMenu.RingsExplode();
        PlayButton.gameObject.SetActive(false);
        HelpButton.gameObject.SetActive(false);
        CreditButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
    }

    public void OnBackButtonClick()
    {
        WholeBoat_BoatMenu.ScoreRingRecover();
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
}