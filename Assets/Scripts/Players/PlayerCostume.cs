using UnityEngine;
using System.Collections;

public class PlayerCostume : MonoBehaviour
{
    [SerializeField] private Renderer BodyRenderer;
    [SerializeField] private Renderer[] CostumeMeshRenderers;
    [SerializeField] private Renderer[] TeamMeshRenderers;
    [SerializeField] private Material[] CostumeMaterials;
    [SerializeField] private Material[] TeamCostumeMaterials;

    public void Initialize(PlayerNumber playerNumber, TeamNumber teamNumber)
    {
        Material[] mats = BodyRenderer.materials;

        if ((int) playerNumber < GameManager.MaximalPlayerNumber)
        {
            mats[0] = CostumeMaterials[(int) playerNumber];

            foreach (Renderer mr in CostumeMeshRenderers)
            {
                mr.material = CostumeMaterials[(int) playerNumber];
            }
        }

        foreach (Renderer mr in TeamMeshRenderers)
        {
            mr.material = TeamCostumeMaterials[(int) teamNumber];
        }

        mats[0] = TeamCostumeMaterials[(int) teamNumber];
        BodyRenderer.materials = mats;
    }
}