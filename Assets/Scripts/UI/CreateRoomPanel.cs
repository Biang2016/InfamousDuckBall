using System.Collections;
using UnityEngine;
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
    [SerializeField] private Button CancelButton;

    [SerializeField] private Text StatusText;

    private Coroutine startServerCreateRoomCoroutine;

    public void ConfirmButtonClick()
    {
        if (startServerCreateRoomCoroutine != null)
        {
            StopCoroutine(startServerCreateRoomCoroutine);
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

        if (BoltNetwork.IsRunning)
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
                (BattleTypes) (BattleTypeDropdown.value),
                roomName: RoomNameInput.text,
                PasswordToggle.isOn,
                password: PasswordInput.text.EncodeSHA512(),
                visible: VisibleToggle.isOn);
        };

        StatusText.text = "Preparing the boat ...";
        BoltLauncher.StartServer();

        float tick = 0f;
        while (!BoltNetwork.IsRunning)
        {
            tick += 0.2f;
            yield return new WaitForSeconds(0.2f);

            if (tick > 5f)
            {
                NoticeManager.Instance.ShowInfoPanelCenter("Create room timeout", 0f, 1f);
                break;
            }
        }

        if (BoltNetwork.IsRunning && BoltNetwork.IsServer)
        {
            startServerCreateRoomCoroutine = null;
            ConfirmButton.interactable = false;
            CancelButton.interactable = false;
            StatusText.text = "Ready..";
        }
        else
        {
            startServerCreateRoomCoroutine = null;
            ConfirmButton.interactable = true;
            CancelButton.interactable = true;
            StatusText.text = "";
            CloseUIForm();
        }
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
        ConfirmButton.interactable = true;
        CancelButton.interactable = true;
        base.Display();
    }

    public override void Hide()
    {
        StatusText.text = "";
        ConfirmButton.interactable = true;
        CancelButton.interactable = true;
        base.Hide();
    }
}