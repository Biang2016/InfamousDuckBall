using UnityEngine;
using System.Collections;

public class PlayerCostume : GooseBodyPart
{
    [SerializeField] private Renderer BodyRenderer;
    [SerializeField] private Renderer[] CostumeMeshRenderers;
    [SerializeField] private Renderer[] TeamMeshRenderers;
    [SerializeField] private Material[] CostumeMaterials;
    [SerializeField] private Material[] TeamCostumeMaterials;
    [SerializeField] private Material EnemyMaterial;

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
        else if (playerNumber == PlayerNumber.AI)
        {
            foreach (Renderer mr in CostumeMeshRenderers)
            {
                mr.material = EnemyMaterial;
            }
        }

        foreach (Renderer mr in TeamMeshRenderers)
        {
            mr.material = TeamCostumeMaterials[(int) teamNumber];
        }

        mats[1] = TeamCostumeMaterials[(int) teamNumber];
        BodyRenderer.materials = mats;
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.DPAD_Left])
        {
            ParentPlayerControl.Player.SwitchTeam(-1);
        }

        if (MultiControllerManager.Instance.Controllers[controllerIndex].ButtonUp[ControlButtons.DPAD_Right])
        {
            ParentPlayerControl.Player.SwitchTeam(1);
        }
    }

    protected override void Operate_AI()
    {
    }
}