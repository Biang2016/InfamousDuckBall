using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LobbyPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.Normal,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.ImPenetrable);

        BoltManager.RefreshRoomList = RefreshRoomList;
    }

    [SerializeField] private Transform RoomListContainer;

    [SerializeField] private InputField RoomIDInputField;
    [SerializeField] private Text UserNameText;

    private List<RoomButton> RoomButtons = new List<RoomButton>();

    private void RefreshRoomList(List<RoomInfoToken> roomInfos)
    {
        foreach (RoomButton rb in RoomButtons)
        {
            rb.PoolRecycle();
        }

        RoomButtons.Clear();

        foreach (RoomInfoToken ri in roomInfos)
        {
            RoomButton rb = GameObjectPoolManager.Instance.PoolDict[GameObjectPoolManager.PrefabNames.RoomButton].AllocateGameObject<RoomButton>(RoomListContainer);
            rb.Initialize(ri);
            RoomButtons.Add(rb);
        }
    }

    public void CreateRoom()
    {
        UIManager.Instance.ShowUIForms<CreateRoomPanel>();
    }
}