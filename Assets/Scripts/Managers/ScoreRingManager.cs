using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Bolt;

public class ScoreRingManager : EntityBehaviour<IScoreRingManagerState>
{
    [SerializeField] private ScoreRing[] Team1ScoreRings;
    [SerializeField] private ScoreRing[] Team2ScoreRings;

    public const int MaxRingNumber = 5;

    private SortedDictionary<TeamNumber, ScoreRing[]> TeamScoreRings = new SortedDictionary<TeamNumber, ScoreRing[]>();

    private ScoreRing[] GetScoreRings(TeamNumber teamNumber)
    {
        if (state.RevertColor)
        {
            return TeamScoreRings[teamNumber == TeamNumber.Team1 ? TeamNumber.Team2 : TeamNumber.Team1];
        }
        else
        {
            return TeamScoreRings[teamNumber];
        }
    }

    public override void Attached()
    {
        TeamScoreRings.Add(TeamNumber.Team1, Team1ScoreRings);
        TeamScoreRings.Add(TeamNumber.Team2, Team2ScoreRings);

        SetTeamRingNumber(MaxRingNumber, MaxRingNumber);
    }

    public void Reset()
    {
        if (BoltNetwork.IsServer)
        {
            state.RingNumber_Team1 = MaxRingNumber;
            state.RingNumber_Team2 = MaxRingNumber;
        }
    }

    void Update()
    {
        SetTeamRingNumber(state.RingNumber_Team1, state.RingNumber_Team2);
    }

    public CostumeType GetRingCostumeType(TeamNumber teamNumber)
    {
        switch (teamNumber)
        {
            case TeamNumber.Team1:
            {
                int overflow_Team1 = state.RingNumber_Team1 > MaxRingNumber ? state.RingNumber_Team1 - MaxRingNumber : 0;
                if (overflow_Team1 > 0)
                {
                    ScoreRing[] srs = GetScoreRings(TeamNumber.Team2);
                    return srs[MaxRingNumber - overflow_Team1].CostumeType;
                }
                else
                {
                    if (state.RingNumber_Team1 == 0)
                    {
                        return CostumeType.Costume1;
                    }
                    else
                    {
                        ScoreRing[] srs = GetScoreRings(TeamNumber.Team1);
                        return srs[state.RingNumber_Team1 - 1].CostumeType;
                    }
                }
            }
            case TeamNumber.Team2:
            {
                int overflow_Team2 = state.RingNumber_Team2 > MaxRingNumber ? state.RingNumber_Team2 - MaxRingNumber : 0;
                if (overflow_Team2 > 0)
                {
                    ScoreRing[] srs = GetScoreRings(TeamNumber.Team1);
                    return srs[MaxRingNumber - overflow_Team2].CostumeType;
                }
                else
                {
                    if (state.RingNumber_Team2 == 0)
                    {
                        return CostumeType.Costume1;
                    }
                    else
                    {
                        ScoreRing[] srs = GetScoreRings(TeamNumber.Team2);
                        return srs[state.RingNumber_Team2 - 1].CostumeType;
                    }
                }
            }
        }

        return CostumeType.Costume1;
    }

    private void SetTeamRingNumber(int ringNumber_Team1, int ringNumber_Team2)
    {
        int overflow_Team1 = ringNumber_Team1 > MaxRingNumber ? ringNumber_Team1 - MaxRingNumber : 0;
        int overflow_Team2 = ringNumber_Team2 > MaxRingNumber ? ringNumber_Team2 - MaxRingNumber : 0;

        ScoreRing[] srs = GetScoreRings(TeamNumber.Team1);
        for (int i = 0; i < srs.Length; i++)
        {
            TeamNumber color = TeamNumber.None;
            if (i < ringNumber_Team1)
            {
                color = TeamNumber.Team1;
            }
            else
            {
                if (overflow_Team2 > 0 && i >= (MaxRingNumber - overflow_Team2))
                {
                    color = TeamNumber.Team2;
                }
            }

            srs[i].Renderer.gameObject.SetActive(color != TeamNumber.None);
            if (color != TeamNumber.None)
            {
                srs[i].Initialize(color, (CostumeType) (i % ConfigManager.CostumeTypeCount));
            }
        }

        srs = GetScoreRings(TeamNumber.Team2);
        for (int i = 0; i < srs.Length; i++)
        {
            TeamNumber color = TeamNumber.None;
            if (i < ringNumber_Team2)
            {
                color = TeamNumber.Team2;
            }
            else
            {
                if (overflow_Team1 > 0 && i >= (MaxRingNumber - overflow_Team1))
                {
                    color = TeamNumber.Team1;
                }
            }

            srs[i].Renderer.gameObject.SetActive(color != TeamNumber.None);
            if (color != TeamNumber.None)
            {
                srs[i].Initialize(color, (CostumeType) (i % ConfigManager.CostumeTypeCount));
            }
        }
    }
}