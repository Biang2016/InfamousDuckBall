using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalPanel : MonoBehaviour
{
    [SerializeField] private Button Button_1v1;
    [SerializeField] private Button Button_2v2;
    [SerializeField] private Button BackButton;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnButton_1v1Click()
    {
        SceneManager.LoadScene("Battle_Smash");
    }

    public void OnButton_2v2Click()
    {
        SceneManager.LoadScene("Battle_FlagRace");
    }

    public void OnBackButtonClick()
    {
        BoatMenuManager.Instance.CameraPosSwitch(BoatMenuManager.CameraPos.SidePerspective);
        Hide();
        BoatMenuManager.Instance.StartMenuPanel_Boat.ButtonGroup_Play.Show();
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