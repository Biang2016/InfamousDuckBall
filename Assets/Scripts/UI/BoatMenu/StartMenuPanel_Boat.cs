using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BoltInternal;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMenuPanel_Boat : MonoBehaviour
{
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button BackButton;

    public StartMenuButtonGroup ButtonGroup_Main;
    public StartMenuButtonGroup ButtonGroup_Play;

    void Start()
    {
        ButtonGroup_Play.Hide(false,false);
    }

    public void OnPlayButtonClick()
    {
        ButtonGroup_Play.Show();
        ButtonGroup_Main.Hide(false,true);
        ExitButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        BackButton.onClick.AddListener(OnBackButtonClick_FromLocalToMain);
        BackButton.onClick.RemoveListener(OnBackButtonClick_FromCreditToMain);
    }

    public void OnHelpButtonClick()
    {
        BoatMenuManager.Instance.CameraPosSwitch(BoatMenuManager.CameraPos.UpDownPerspective);
        BoatMenuManager.Instance.HelpPanel.Display();
    }

    public void OnCreditButtonClick()
    {
        ButtonGroup_Main.Hide(true, true);
        ExitButton.gameObject.SetActive(false);
        BackButton.onClick.RemoveListener(OnBackButtonClick_FromLocalToMain);
        BackButton.onClick.AddListener(OnBackButtonClick_FromCreditToMain);
        BackButton.gameObject.SetActive(true);
    }

    public void OnLocalButtonClick()
    {
        ButtonGroup_Play.Hide(true, true);
        BoatMenuManager.Instance.CameraPosSwitch(BoatMenuManager.CameraPos.UpDownPerspective);
        BoatMenuManager.Instance.LocalPanel.Display();
        GameManager.Instance.SwitchNetworkMode(GameManager.NetworkMode.Local);
    }

    public void OnOnlineButtonClick()
    {
        UIManager.Instance.ShowUIForms<SelectRegionPanel>();
    }

    public void OnBackButtonClick_FromCreditToMain()
    {
        ButtonGroup_Main.Show();
        ExitButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);
    }

    public void OnBackButtonClick_FromLocalToMain()
    {
        ButtonGroup_Main.Show();
        ButtonGroup_Play.Hide(false, true);
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

    [Serializable]
    public class StartMenuButton
    {
        public Button Button;
        public ScoreRingSingleBoatMenu ScoreRing;

        public void Show()
        {
            Button.gameObject.SetActive(true);
            ScoreRing.Recover();
        }

        public void Hide(bool withSound, bool withFX)
        {
            Button.gameObject.SetActive(false);
            ScoreRing.Explode(withSound, withFX);
        }
    }

    [Serializable]
    public class StartMenuButtonGroup
    {
        public List<StartMenuButton> StartMenuButtons = new List<StartMenuButton>();

        public void Show()
        {
            foreach (StartMenuButton button in StartMenuButtons)
            {
                button.Show();
            }
        }

        public void Hide(bool withSound, bool withFX)
        {
            foreach (StartMenuButton button in StartMenuButtons)
            {
                button.Hide(withSound, withFX);
            }
        }
    }
}