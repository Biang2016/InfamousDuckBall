using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateRoomPanel : BaseUIForm
{
    void Awake()
    {
        UIType.InitUIType(
            isClearStack: false,
            isESCClose: false,
            isClickElsewhereClose: false,
            uiForms_Type: UIFormTypes.PopUp,
            uiForms_ShowMode: UIFormShowModes.Normal,
            uiForm_LucencyType: UIFormLucencyTypes.Blur);
    }

    [SerializeField] private InputField RoomNameInput;
    [SerializeField] private Toggle PasswordToggle;
    [SerializeField] private InputField PasswordInput;
    [SerializeField] private Toggle VisibleToggle;
    [SerializeField] private Dropdown BattleTypeDropdown;

    [SerializeField] private Button ConfirmButton;

    [SerializeField] private Text StatusText;

    private Coroutine startServerCreateRoomCoroutine;

    public void ConfirmButtonClick()
    {
        if (startServerCreateRoomCoroutine != null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(RoomNameInput.text))
        {
            startServerCreateRoomCoroutine = StartCoroutine(Co_StartServerCreateRoom());
        }
        else
        {
            NoticeManager.Instance.ShowInfoPanelCenter("Invalid room name", 0f, 1f);
        }
    }

    IEnumerator Co_StartServerCreateRoom()
    {
        ConfirmButton.interactable = false;

        if (BoltNetwork.IsRunning && BoltNetwork.IsClient)
        {
            BoltNetwork.Shutdown();
            StatusText.text = "Switching to server mode ...";
        }

        while (BoltNetwork.IsRunning)
        {
            yield return new WaitForSeconds(0.2f);
        }

        BoltManager.OnBoltStartDone_Server = delegate
        {
            BoltManager.StartServerSession(
                (BattleTypes) (BattleTypeDropdown.value + 1),
                roomName: RoomNameInput.text,
                password: PasswordInput.text.EncodeSHA512(),
                visible: VisibleToggle.isOn);
        };

        StatusText.text = "Preparing the boat ...";
        BoltLauncher.StartServer();
        yield return new WaitForSeconds(1f);
        startServerCreateRoomCoroutine = null;
        ConfirmButton.interactable = true;
        StatusText.text = "Ready..";
    }

    public void CancelButtonClick()
    {
        if (startServerCreateRoomCoroutine != null)
        {
            StopCoroutine(startServerCreateRoomCoroutine);
            BoltManager.OnBoltStartDone_Server = null;
            if (BoltNetwork.IsRunning && BoltNetwork.IsServer)
            {
                BoltNetwork.ShutdownImmediate();
                BoltLauncher.StartClient();
            }

            ConfirmButton.interactable = true;
        }

        StatusText.text = "";
        CloseUIForm();
    }

    public override void Display()
    {
        StatusText.text = "";
        base.Display();
    }

    public override void Hide()
    {
        StatusText.text = "";
        base.Hide();
    }
}