using UnityEngine;
using System.Collections;
using Bolt;
using Bolt.Matchmaking;
using Bolt.Utils;
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

    public void Initialize()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (BoltMatchmaking.CurrentSession != null)
            {
                IProtocolToken token = BoltMatchmaking.CurrentSession.GetProtocolToken();
                if (token != null && token is RoomInfoToken rit)
                {
                    RoomIDText.text = "Room ID: " + rit.RoomName;
                }
            }
        }
        else
        {
            RoomIDText.text = "";
        }
    }

    public void OnContinueButtonClick()
    {
        CloseUIForm();
    }

    public void OnLeaveButtonClick()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
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
        }
        else
        {
            GameManager.Instance.ReturnToLobby();
        }

        CloseUIForm();
    }
}