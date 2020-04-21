﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigRows : MonoBehaviour
{
    private List<ConfigRow> configRows = new List<ConfigRow>();

    public void Initialize()
    {
        foreach (ConfigRow cr in configRows)
        {
            cr.PoolRecycle();
        }

        configRows.Clear();
        foreach (string s in Enum.GetNames(typeof(ConfigRow.ConfigRowType)))
        {
            ConfigRow.ConfigRowType t = (ConfigRow.ConfigRowType) Enum.Parse(typeof(ConfigRow.ConfigRowType), s);
            ConfigRow cr = GameObjectPoolManager.Instance.PoolDict[GameObjectPoolManager.PrefabNames.ConfigRow].AllocateGameObject<ConfigRow>(transform);
            cr.Initialize(t, 0.1f, 3f);
            configRows.Add(cr);
        }
    }

    public void Refresh()
    {
        foreach (ConfigRow configRow in configRows)
        {
            configRow.Refresh();
        }
    }
}