using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreRingManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] Team1ScoreRings;
    [SerializeField] private MeshRenderer[] Team2ScoreRings;

    public const int MaxRingNumber = 5;

    private SortedDictionary<TeamNumber, MeshRenderer[]> TeamScoreRings = new SortedDictionary<TeamNumber, MeshRenderer[]>();
    private SortedDictionary<TeamNumber, int> TeamScoreDict = new SortedDictionary<TeamNumber, int>();

    void Awake()
    {
        TeamScoreRings.Add(TeamNumber.Team1, Team1ScoreRings);
        TeamScoreRings.Add(TeamNumber.Team2, Team2ScoreRings);
    }

    public void Reset()
    {
        foreach (KeyValuePair<TeamNumber, MeshRenderer[]> kv in TeamScoreRings)
        {
            SetTeamRingNumber(kv.Key, MaxRingNumber);
        }
    }

    public bool GetRing(TeamNumber teamNumber, out Material ringMaterial)
    {
        if (TeamScoreDict[teamNumber] == 0)
        {
            // Todo lost
            ringMaterial = null;
            return false;
        }
        else
        {
            SetTeamRingNumber(teamNumber, TeamScoreDict[teamNumber] - 1);
            ringMaterial = TeamScoreRings[teamNumber][TeamScoreDict[teamNumber]].material;
            return true;
        }
    }

    private void SetTeamRingNumber(TeamNumber teamNumber, int ringNumber)
    {
        MeshRenderer[] mrs = TeamScoreRings[teamNumber];
        for (int i = 0; i < mrs.Length; i++)
        {
            mrs[i].enabled = i < ringNumber;
        }

        TeamScoreDict[teamNumber] = ringNumber;
    }
}