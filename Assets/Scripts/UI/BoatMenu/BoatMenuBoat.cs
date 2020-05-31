using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoatMenuBoat : MonoBehaviour
{
    public void OnShowGameLogo()
    {
        UIManager.Instance.ShowUIForms<GameLogoPanel>().GameLogoDrop();
    }

    public void OnShowMakerPanel()
    {
        UIManager.Instance.ShowUIForms<MakerPanel>();
    }
}