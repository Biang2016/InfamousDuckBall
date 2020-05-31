using UnityEngine;
using UdpKit.Platform.Photon;
using UnityEngine.UI;

public class SelectRegionPanel : BaseUIForm
{
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

    [SerializeField] private Dropdown RegionDropdown;
    [SerializeField] private Animator Anim;

    void Start()
    {
        RegionDropdown.options.Clear();
        foreach (PhotonRegion.Regions region in Regions.AvailableRegions)
        {
            RegionDropdown.options.Add(new Dropdown.OptionData(PhotonRegion.GetRegion(region).Name));
        }

        RegionDropdown.onValueChanged.AddListener(delegate(int index) { Regions.CurSelectedRegion = Regions.AvailableRegions[index]; });
    }

    public override void Display()
    {
        base.Display();
        Anim.SetTrigger("Show");
        BoatMenuManager.Instance.CameraPosSwitch(BoatMenuManager.CameraPos.UpDownPerspective);
    }

    public void OnConfirmButtonClick()
    {
        if (Regions.SwitchRegion(Regions.CurSelectedRegion))
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                {
                    NoticeManager.Instance.ShowInfoPanelCenter("Internet Not Reachable.", 0f, 1f);
                    BoatMenuManager.Instance.FromLobbyBackToStartMenu();
                    GameManager.Instance.SwitchNetworkMode(GameManager.NetworkMode.Local);
                    break;
                }
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                {
                    BoatMenuManager.Instance.LobbyPanel.Display();
                    GameManager.Instance.SwitchNetworkMode(GameManager.NetworkMode.Online);
                    break;
                }
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                {
                    BoatMenuManager.Instance.LobbyPanel.Display();
                    GameManager.Instance.SwitchNetworkMode(GameManager.NetworkMode.Online);
                    break;
                }
            }

            CloseUIForm();
        }
        else
        {
            NoticeManager.Instance.ShowInfoPanelCenter("Switch Region failed, Try again", 0, 1.5f);
            if (BoltNetwork.IsRunning)
            {
                BoltNetwork.ShutdownImmediate();
            }

            GameManager.Instance.SwitchNetworkMode(GameManager.NetworkMode.Local);
            return;
        }
    }
}