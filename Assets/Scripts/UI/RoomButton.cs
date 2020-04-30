using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomButton : PoolObject
{
    [SerializeField] private Button Button;
    [SerializeField] private Text RoomNameText;
    [SerializeField] private Text BattleTypeText;
    [SerializeField] private Text PlayerNumberText;
    [SerializeField] private Image HasPasswordIcon;
    [SerializeField] private Text StatusText;

    public RoomInfoToken MyRoomInfo;

    public void Initialize(RoomInfoToken roomInfo)
    {
        Button.onClick.RemoveAllListeners();
        if (roomInfo.OnRoomButtonClick != null)
        {
            Button.onClick.AddListener(roomInfo.OnRoomButtonClick);
        }

        MyRoomInfo = roomInfo;
        RoomNameText.text = roomInfo.RoomName;
        BattleTypeText.text = roomInfo.BattleType.ToString();
        PlayerNumberText.color = roomInfo.Cur_PlayerNumber == roomInfo.Max_PlayerNumber ? Color.red : Color.green;
        PlayerNumberText.text = roomInfo.Cur_PlayerNumber + " / " + roomInfo.Max_PlayerNumber;
        StatusText.text = roomInfo.M_Status.ToString();
        switch (roomInfo.M_Status)
        {
            case RoomInfoToken.Status.Waiting:
            {
                StatusText.color = Color.green;
                break;
            }
            case RoomInfoToken.Status.Full:
            {
                StatusText.color = ClientUtils.HTMLColorToColor("#FF8000");
                break;
            }
            case RoomInfoToken.Status.Playing:
            {
                StatusText.color = Color.red;
                break;
            }
        }

        HasPasswordIcon.enabled = roomInfo.HasPassword;
    }
}