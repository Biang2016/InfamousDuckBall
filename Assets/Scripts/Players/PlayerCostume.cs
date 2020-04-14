using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCostume : MonoBehaviour
{
    [SerializeField] private Renderer[] CostumeMeshRenderers;
    [SerializeField] private Material[] Team1CostumeMaterials;
    [SerializeField] private Material[] Team2CostumeMaterials;

    private SortedDictionary<TeamNumber, Material[]> TeamCostumeMatDict = new SortedDictionary<TeamNumber, Material[]>();

    void Awake()
    {
        TeamCostumeMatDict.Add(TeamNumber.Team1, Team1CostumeMaterials);
        TeamCostumeMatDict.Add(TeamNumber.Team2, Team2CostumeMaterials);
    }

    public void Initialize(PlayerNumber playerNumber, TeamNumber teamNumber, CostumeType costumeType)
    {
        Material mat = TeamCostumeMatDict[teamNumber][(int) costumeType];

        if ((int) playerNumber < GameManager.MaximalPlayerNumber)
        {
            foreach (Renderer mr in CostumeMeshRenderers)
            {
                mr.material = mat;
            }
        }
    }
}

public enum CostumeType
{
    Costume1 = 0,
    Costume2 = 1,
    Costume3 = 2,
}