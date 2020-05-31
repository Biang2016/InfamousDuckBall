using UdpKit.Platform;
using UdpKit.Platform.Photon;
using UnityEngine;

public class Regions
{
    public static PhotonRegion.Regions[] AvailableRegions = new PhotonRegion.Regions[]
    {
        PhotonRegion.Regions.BEST_REGION,
        PhotonRegion.Regions.ASIA,
        PhotonRegion.Regions.AU,
        PhotonRegion.Regions.CAE,
        PhotonRegion.Regions.EU,
        PhotonRegion.Regions.IN,
        PhotonRegion.Regions.JP,
        PhotonRegion.Regions.KR,
        PhotonRegion.Regions.RU,
        PhotonRegion.Regions.RUE,
        PhotonRegion.Regions.SA,
        PhotonRegion.Regions.US,
        PhotonRegion.Regions.USW,
    };



    public static PhotonRegion.Regions CurSelectedRegion;

    public static bool SwitchRegion(PhotonRegion.Regions region)
    {
        if (BoltNetwork.IsRunning == false)
        {
            // Get the current Region based on the index
            PhotonRegion targetRegion = PhotonRegion.GetRegion(CurSelectedRegion);

            // Update the target region
            BoltRuntimeSettings.instance.UpdateBestRegion(targetRegion);

            // IMPORTANT
            // Initialize the Photon Platform again
            // this will update the internal cached region
            BoltLauncher.SetUdpPlatform(new PhotonPlatform());
            return true;
        }
        else
        {
            BoltLog.Error("Bolt is running, you can't change region while running");
            return false;
        }
    }
}