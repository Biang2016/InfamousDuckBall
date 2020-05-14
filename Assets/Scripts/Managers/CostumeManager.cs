using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : MonoSingleton<CostumeManager>
{
    private SortedDictionary<TeamNumber, Texture[]> TeamTextureDict = new SortedDictionary<TeamNumber, Texture[]>();
    [SerializeField] private Texture[] Team1_Textures;
    [SerializeField] private Texture[] Team2_Textures;

    void Awake()
    {
        TeamTextureDict.Add(TeamNumber.Team1, Team1_Textures);
        TeamTextureDict.Add(TeamNumber.Team2, Team2_Textures);
    }

    public void ChangeCostume(Renderer renderer, TeamNumber teamNumber, CostumeType costumeType)
    {
        Texture tex = TeamTextureDict[teamNumber][(int) costumeType];
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetTexture("_MainTex", tex);
        renderer.SetPropertyBlock(mpb);
    }
}