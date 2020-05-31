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
    public LocalPanel LocalPanel;
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

    public enum CameraPos
    {
        SidePerspective,
        UpDownPerspective,
    }

    private CameraPos cur_CameraPos = CameraPos.SidePerspective;

    public void CameraPosSwitch(CameraPos pos)
    {
        if (pos != cur_CameraPos)
        {
            switch (pos)
            {
                case CameraPos.SidePerspective:
                {
                    GameManager.Instance.GameLogoPanel.GameLogoDrop();
                    StartMenuPanel_Boat.gameObject.SetActive(true);
                    BoatMenuCamera.transform.DOMove(StartMenuCameraPivot.position, 0.5f);
                    BoatMenuCamera.transform.DORotateQuaternion(StartMenuCameraPivot.rotation, 0.5f);
                    UIManager.Instance.ShowUIForms<MakerPanel>();
                    break;
                }
                case CameraPos.UpDownPerspective:
                {
                    GameManager.Instance.GameLogoPanel.GameLogoPullUp();
                    StartMenuPanel_Boat.gameObject.SetActive(false);
                    BoatMenuCamera.transform.DOMove(LobbyPanelCameraPivot.position, 0.5f);
                    BoatMenuCamera.transform.DORotateQuaternion(LobbyPanelCameraPivot.rotation, 0.5f);
                    UIManager.Instance.CloseUIForm<MakerPanel>();
                    break;
                }
            }

            cur_CameraPos = pos;
        }
    }

    public void FromStartMenuToLobby()
    {
        LobbyPanel.Display();
    }

    public void FromLobbyBackToStartMenu()
    {
        LobbyPanel.Hide();
        CameraPosSwitch(CameraPos.SidePerspective);
        StartMenuPanel_Boat.ButtonGroup_Play.Show();
    }

    public void FromStartMenuToHelp()
    {
    }
}