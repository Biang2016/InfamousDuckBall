using UnityEngine;
using System.Collections;

public class PlayerCostume : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] CostumeMeshRenderers;
    [SerializeField] private Material[] CostumeMaterials;

    public void Initialize(PlayerNumber playerNumber)
    {
        foreach (MeshRenderer mr in CostumeMeshRenderers)
        {
            mr.material = CostumeMaterials[(int) playerNumber];
        }
    }
}