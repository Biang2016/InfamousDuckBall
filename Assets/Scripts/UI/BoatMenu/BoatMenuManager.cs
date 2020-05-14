using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class BoatMenuManager : MonoSingleton<BoatMenuManager>
{
    public BoatMenuBoat BoatMenuBoat;
    public StartMenuPanel_Boat StartMenuPanel_Boat;
    public LobbyPanel LobbyPanel;
    public HelpPanel HelpPanel;
    [SerializeField] private Animator BoatAnim;

    [SerializeField] private Transform StartMenuCameraPivot;
    [SerializeField] private Transform LobbyPanelCameraPivot;
    [SerializeField] private Camera BoatMenuCamera;

    public void BoatMoveIn()
    {
        BoatAnim.SetTrigger("MoveIn");
    }

    public void BoatMoveInWithoutGameLogoPanel()
    {
        BoatAnim.SetTrigger("MoveInPure");
    }

    public void FromStartMenuToLobby()
    {
        BoatMenuBoat.ScoreRingsExplode();
        StartMenuPanel_Boat.gameObject.SetActive(false);
        GameManager.Instance.GameLogoPanel.GameLogoPullUp();
        LobbyPanel.Display();
        BoatMenuCamera.transform.DOMove(LobbyPanelCameraPivot.position, 0.5f);
        BoatMenuCamera.transform.DORotateQuaternion(LobbyPanelCameraPivot.rotation, 0.5f);

        UIManager.Instance.CloseUIForm<MakerPanel>();
    }

    public void FromLobbyBackToStartMenu()
    {
        BoatMenuBoat.ScoreRingRecover();
        StartMenuPanel_Boat.gameObject.SetActive(true);
        GameManager.Instance.GameLogoPanel.GameLogoDrop();
        GameManager.Instance.LobbyPanel.Hide();
        BoatMenuCamera.transform.DOMove(StartMenuCameraPivot.position, 0.5f);
        BoatMenuCamera.transform.DORotateQuaternion(StartMenuCameraPivot.rotation, 0.5f);

        UIManager.Instance.ShowUIForms<MakerPanel>();

    }

    public void FromStartMenuToHelp()
    {
        BoatMenuBoat.ScoreRingsExplode();
        StartMenuPanel_Boat.gameObject.SetActive(false);
        GameManager.Instance.GameLogoPanel.GameLogoPullUp();
        HelpPanel.Display();
        BoatMenuCamera.transform.DOMove(LobbyPanelCameraPivot.position, 0.5f);
        BoatMenuCamera.transform.DORotateQuaternion(LobbyPanelCameraPivot.rotation, 0.5f);

        UIManager.Instance.CloseUIForm<MakerPanel>();
    }

    public void FromHelpBackToStartMenu()
    {
        BoatMenuBoat.ScoreRingRecover();
        StartMenuPanel_Boat.gameObject.SetActive(true);
        GameManager.Instance.GameLogoPanel.GameLogoDrop();
        GameManager.Instance.HelpPanel.Hide();
        BoatMenuCamera.transform.DOMove(StartMenuCameraPivot.position, 0.5f);
        BoatMenuCamera.transform.DORotateQuaternion(StartMenuCameraPivot.rotation, 0.5f);

        UIManager.Instance.ShowUIForms<MakerPanel>();
    }
}