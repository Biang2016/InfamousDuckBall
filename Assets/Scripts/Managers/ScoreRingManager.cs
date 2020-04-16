using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreRingManager : MonoBehaviour
{
    [SerializeField] private ScoreRing[] Team1ScoreRings;
    [SerializeField] private ScoreRing[] Team2ScoreRings;

    public const int MaxRingNumber = 5;

    private SortedDictionary<TeamNumber, ScoreRing[]> TeamScoreRings = new SortedDictionary<TeamNumber, ScoreRing[]>();

    void Awake()
    {
        TeamScoreRings.Add(TeamNumber.Team1, Team1ScoreRings);
        TeamScoreRings.Add(TeamNumber.Team2, Team2ScoreRings);
        Reset();
    }

    public void Reset()
    {
        foreach (KeyValuePair<TeamNumber, ScoreRing[]> kv in TeamScoreRings)
        {
            SetTeamRingNumber(kv.Key, MaxRingNumber);
            for (int i = 0; i < kv.Value.Length; i++)
            {
                ScoreRing sr = kv.Value[i];
                sr.Initialize(kv.Key, (CostumeType) (i % ConfigManager.CostumeTypeCount));
            }
        }
    }

    public CostumeType GetRingCostumeType(TeamNumber teamNumber)
    {
        return TeamScoreRings[teamNumber][GameManager.Cur_BattleManager.TeamDict[teamNumber].Score - 1].CostumeType;
    }

    public void SetTeamRingNumber(TeamNumber teamNumber, int ringNumber)
    {
        ScoreRing[] srs = TeamScoreRings[teamNumber];
        for (int i = 0; i < srs.Length; i++)
        {
            srs[i].Renderer.gameObject.SetActive(i < ringNumber);
        }
    }
}