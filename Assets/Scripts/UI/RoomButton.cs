using UnityEngine;
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

        switch (roomInfo.BattleType)
        {
            case BattleTypes.Smash:
            {
                BattleTypeText.text = "Smash 1v1";
                break;
            }
            case BattleTypes.FlagRace:
            {
                BattleTypeText.text = "Ring-Race 2v2";
                break;
            }
        }

        PlayerNumberText.color = roomInfo.Cur_PlayerNumber == roomInfo.Max_PlayerNumber ? ClientUtils.HTMLColorToColor("#d36c6c") : ClientUtils.HTMLColorToColor("#6cd383");
        PlayerNumberText.text = roomInfo.Cur_PlayerNumber + " / " + roomInfo.Max_PlayerNumber;
        StatusText.text = roomInfo.M_Status.ToString();
        switch (roomInfo.M_Status)
        {
            case RoomInfoToken.Status.Waiting:
            {
                StatusText.color = ClientUtils.HTMLColorToColor("#6cd383");
                break;
            }
            case RoomInfoToken.Status.Full:
            {
                StatusText.color = ClientUtils.HTMLColorToColor("#D3B46C");
                break;
            }
            case RoomInfoToken.Status.Playing:
            {
                StatusText.color = ClientUtils.HTMLColorToColor("#d36c6c");
                break;
            }
            case RoomInfoToken.Status.Closing:
            {
                StatusText.color = Color.gray;
                break;
            }
        }

        HasPasswordIcon.enabled = roomInfo.HasPassword;
    }
}