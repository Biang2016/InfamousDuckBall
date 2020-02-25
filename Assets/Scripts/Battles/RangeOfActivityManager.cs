using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangeOfActivityManager : MonoBehaviour
{
    private List<RangeOfActivity> ForbiddenRanges = new List<RangeOfActivity>();

    void Awake()
    {
        foreach (RangeOfActivity roa in GetComponentsInChildren<RangeOfActivity>())
        {
            if (roa.M_RangeType == RangeOfActivity.RangeType.Forbidden)
            {
                ForbiddenRanges.Add(roa);
            }
        }
    }

    public void AddForbidArea(RangeOfActivity roa)
    {
        ForbiddenRanges.Add(roa);
    }

    public void RemoveForbidArea(RangeOfActivity roa)
    {
        ForbiddenRanges.Remove(roa);
    }
}