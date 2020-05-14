using UnityEngine;

public class PlayerCostume : MonoBehaviour
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponent<Duck>();
    }

    [SerializeField] private Renderer[] CostumeMeshRenderers;

    public void Initialize(PlayerNumber playerNumber, TeamNumber teamNumber, CostumeType costumeType)
    {
        foreach (Renderer renderer in CostumeMeshRenderers)
        {
            CostumeManager.Instance.ChangeCostume(renderer, teamNumber, costumeType);
        }

        Duck.SunGlasses.Initialize(teamNumber);
    }
}

public enum CostumeType
{
    Costume1 = 0,
    Costume2 = 1,
    Costume3 = 2,
    None = -1,
}