﻿using System.Collections.Generic;
using UnityEngine;

public class SunGlasses : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    [SerializeField] private Animator Anim;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    [SerializeField] private Renderer[] Renderers;
    [SerializeField] private Material Team1_Mat;
    [SerializeField] private Material Team2_Mat;

    private SortedDictionary<TeamNumber, Material> TeamMatDict = new SortedDictionary<TeamNumber, Material>();

    void Awake()
    {
        TeamMatDict.Add(TeamNumber.Team1, Team1_Mat);
        TeamMatDict.Add(TeamNumber.Team2, Team2_Mat);
    }

    public void Initialize(TeamNumber teamNumber)
    {
        foreach (Renderer r in Renderers)
        {
            r.material = TeamMatDict[teamNumber];
        }
    }

    public void Charging()
    {
        Anim.SetTrigger("SwitchDown");
    }

    public void Normal()
    {
        Anim.SetTrigger("SwitchUp");
    }
}