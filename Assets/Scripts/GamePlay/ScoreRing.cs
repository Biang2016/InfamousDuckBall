using UnityEngine;

public class ScoreRing : MonoBehaviour
{
    public Renderer Renderer;
    internal TeamNumber TeamNumber = TeamNumber.None;
    internal CostumeType CostumeType = CostumeType.None;

    public void Initialize(TeamNumber teamNumber, CostumeType costumeType)
    {
        if (CostumeType == costumeType && TeamNumber == teamNumber)
        {
            return;
        }

        CostumeType = costumeType;
        CostumeManager.Instance.ChangeCostume(Renderer, teamNumber, costumeType);
    }
}