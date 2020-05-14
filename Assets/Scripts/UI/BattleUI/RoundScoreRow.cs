using UnityEngine;
using System.Collections;

public class RoundScoreRow : MonoBehaviour
{
    public RoundScorePanel RoundScorePanel;

    public void OnBlueTextReady()
    {
        RoundScorePanel.OnBlueTextReady();
    }

    public void OnRedTextReady()
    {
        RoundScorePanel.OnRedTextReady();
    }

    public void OnAnimEnds()
    {
        RoundScorePanel.CloseUIForm();
    }
}