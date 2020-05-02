using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WholeBoat_BoatMenu : MonoBehaviour
{
    public void OnBoatArrive()
    {
        StartMenu sm = UIManager.Instance.ShowUIForms<StartMenu>();
        sm.GameLogoDrop();
    }

    public List<ScoreRingSingleBoatMenu> ScoreRings = new List<ScoreRingSingleBoatMenu>();

    public void RingsExplode()
    {
        foreach (ScoreRingSingleBoatMenu sr in ScoreRings)
        {
            sr.Explode();
        }
    }

    public void ScoreRingRecover()
    {
        foreach (ScoreRingSingleBoatMenu sr in ScoreRings)
        {
            sr.Recover();
        }
    }
}