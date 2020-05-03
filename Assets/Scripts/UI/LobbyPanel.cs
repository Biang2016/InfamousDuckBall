using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    void Awake()
    {
        BoltManager.RefreshRoomListInUI = RefreshRoomList;
    }

    [SerializeField] private Transform RoomListContainer;

    [SerializeField] private InputField RoomIDInputField;
    [SerializeField] private Text UserNameText;
    [SerializeField] private InputField SearchInputField;

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

    void Start()
    {
        InvokeRepeating("RefreshButtonClick", 0f, 3f);
    }

    public void Display()
    {
        gameObject.SetActive(true);
        UserNameText.text = PlayerPrefs.GetString("PlayerID");
        RefreshButtonClick();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RefreshButtonClick()
    {
        //SearchInputField.text
        BoltManager.UpdateRoomList(BoltNetwork.SessionList);
    }

    public void OnBackButtonClick()
    {
        BoatMenuManager.Instance.FromLobbyBackToStartMenu();
    }
}