using UnityEngine;
using System.Collections;

public class PlayerCostume : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] CostumeMeshRenderers;
    [SerializeField] private SkinnedMeshRenderer[] SkinnedMeshRenderers;
    [SerializeField] private Material[] CostumeMaterials;
    [SerializeField] private Material EnemyMaterial;

    public void Initialize(PlayerNumber playerNumber)
    {
        if ((int) playerNumber < GameManager.MaximalPlayerNumber)
        {
            foreach (MeshRenderer mr in CostumeMeshRenderers)
            {
                mr.material = CostumeMaterials[(int) playerNumber];
            }

            foreach (SkinnedMeshRenderer smr in SkinnedMeshRenderers)
            {
                smr.material = CostumeMaterials[(int) playerNumber];
            }
        }
        else if (playerNumber == PlayerNumber.AI)
        {
            foreach (MeshRenderer mr in CostumeMeshRenderers)
            {
                mr.material = EnemyMaterial;
            }

            foreach (SkinnedMeshRenderer smr in SkinnedMeshRenderers)
            {
                smr.material = CostumeMaterials[(int) playerNumber];
            }
        }
    }
}