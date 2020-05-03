using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoatMenuBoat : MonoBehaviour
{
    public void OnBoatArrive()
    {
        GameLogoPanel sm = UIManager.Instance.ShowUIForms<GameLogoPanel>();
        sm.GameLogoDrop();
    }

    public List<ScoreRingSingleBoatMenu> ScoreRings = new List<ScoreRingSingleBoatMenu>();

    public void ScoreRingsExplode()
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