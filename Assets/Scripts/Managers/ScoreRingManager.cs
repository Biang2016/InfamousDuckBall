using System.Collections.Generic;
using Bolt;
using UnityEngine;

public class ScoreRingManager : EntityBehaviour<IScoreRingManagerState>
{
    [SerializeField] private ScoreRing[] Team1ScoreRings;
    [SerializeField] private ScoreRing[] Team2ScoreRings;

    public const int MaxRingNumber = 5;

    private SortedDictionary<TeamNumber, ScoreRing[]> TeamScoreRings = new SortedDictionary<TeamNumber, ScoreRing[]>();

    private int ringNumber_Team1;

    public int RingNumber_Team1
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.RingNumber_Team1;
            }
            else
            {
                return ringNumber_Team1;
            }
        }
        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.RingNumber_Team1 = value;
            }
            else
            {
                ringNumber_Team1 = value;
            }
        }
    }

    private int ringNumber_Team2;

    public int RingNumber_Team2
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.RingNumber_Team2;
            }
            else
            {
                return ringNumber_Team2;
            }
        }

        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.RingNumber_Team2 = value;
            }
            else
            {
                ringNumber_Team2 = value;
            }
        }
    }

    private bool revertColor;

    public bool RevertColor
    {
        get
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                return state.RevertColor;
            }
            else
            {
                return revertColor;
            }
        }

        set
        {
            if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
            {
                state.RevertColor = value;
            }
            else
            {
                revertColor = value;
            }
        }
    }

    private ScoreRing[] GetScoreRings(TeamNumber teamNumber)
    {
        if (RevertColor)
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
        Init();
    }

    void Awake()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local)
        {
            Init();
        }
    }

    void Init()
    {
        TeamScoreRings.Add(TeamNumber.Team1, Team1ScoreRings);
        TeamScoreRings.Add(TeamNumber.Team2, Team2ScoreRings);
        SetTeamRingNumber(MaxRingNumber, MaxRingNumber);
    }

    public void Reset()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            RingNumber_Team1 = MaxRingNumber;
            RingNumber_Team2 = MaxRingNumber;
        }
    }

    void Update()
    {
        SetTeamRingNumber(RingNumber_Team1, RingNumber_Team2);
    }

    public CostumeType GetRingCostumeType(TeamNumber teamNumber)
    {
        switch (teamNumber)
        {
            case TeamNumber.Team1:
            {
                int overflow_Team1 = RingNumber_Team1 > MaxRingNumber ? RingNumber_Team1 - MaxRingNumber : 0;
                if (overflow_Team1 > 0)
                {
                    ScoreRing[] srs = GetScoreRings(TeamNumber.Team2);
                    return srs[MaxRingNumber - overflow_Team1].CostumeType;
                }
                else
                {
                    if (RingNumber_Team1 == 0)
                    {
                        return CostumeType.Costume1;
                    }
                    else
                    {
                        ScoreRing[] srs = GetScoreRings(TeamNumber.Team1);
                        return srs[RingNumber_Team1 - 1].CostumeType;
                    }
                }
            }
            case TeamNumber.Team2:
            {
                int overflow_Team2 = RingNumber_Team2 > MaxRingNumber ? RingNumber_Team2 - MaxRingNumber : 0;
                if (overflow_Team2 > 0)
                {
                    ScoreRing[] srs = GetScoreRings(TeamNumber.Team1);
                    return srs[MaxRingNumber - overflow_Team2].CostumeType;
                }
                else
                {
                    if (RingNumber_Team2 == 0)
                    {
                        return CostumeType.Costume1;
                    }
                    else
                    {
                        ScoreRing[] srs = GetScoreRings(TeamNumber.Team2);
                        return srs[RingNumber_Team2 - 1].CostumeType;
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
        for (int i = 0;
            i < srs.Length;
            i++)
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
        for (int i = 0;
            i < srs.Length;
            i++)
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