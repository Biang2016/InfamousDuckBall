using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaveGamePanel : BaseUIForm
{
    [SerializeField] private Text LeaveRoomText;
    [SerializeField] private Text RoomIDText;

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

    public void OnContinueButtonClick()
    {
        CloseUIForm();
    }

    public void OnLeaveButtonClick()
    {
        if (BoltNetwork.IsServer)
        {
            CloseRoomEvent evnt = CloseRoomEvent.Create();
            evnt.Send();
        }
        else
        {
            GameManager.Instance.ReturnToLobby();
        }

        CloseUIForm();
    }
}