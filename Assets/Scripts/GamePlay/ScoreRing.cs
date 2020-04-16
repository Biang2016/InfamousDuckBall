using UnityEngine;
using System.Collections;

public class ScoreRing : MonoBehaviour
{
    public Renderer Renderer;
    internal CostumeType CostumeType;

    public void Initialize(TeamNumber teamNumber, CostumeType costumeType)
    {
        CostumeType = costumeType;
        CostumeManager.Instance.ChangeCostume(Renderer, teamNumber, costumeType);
    }
}