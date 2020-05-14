using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 自行寻找合适复活点
/// </summary>
public class TeamSpawnPointManager : MonoBehaviour
{
    private Dictionary<TeamNumber, List<TeamSpawnPoint>> TeamSpawnPointDict = new Dictionary<TeamNumber, List<TeamSpawnPoint>>();

    [SerializeField] private bool ConsiderInCamera = true;

    [SerializeField] private float MinDistance = 5;

    void Awake()
    {
        List<TeamSpawnPoint> TeamSpawnPoints = gameObject.GetComponentsInChildren<TeamSpawnPoint>(true).ToList();
        foreach (TeamSpawnPoint sp in TeamSpawnPoints)
        {
            sp.ConsiderInCamera = ConsiderInCamera;
            if (!TeamSpawnPointDict.ContainsKey(sp.AllowedTeamNumber))
            {
                TeamSpawnPointDict.Add(sp.AllowedTeamNumber, new List<TeamSpawnPoint>());
            }

            TeamSpawnPointDict[sp.AllowedTeamNumber].Add(sp);
        }
    }

    public void Spawn(PlayerNumber playerNumber, TeamNumber teamNumber)
    {
        List<Vector3> notValidPoints = GameManager.Instance.Cur_BattleManager.GetAllPlayerPositions();

        List<TeamSpawnPoint> candidates = new List<TeamSpawnPoint>();
        List<TeamSpawnPoint> candidates_any = new List<TeamSpawnPoint>();
        List<IRevivePlayer> results = new List<IRevivePlayer>();

        if (TeamSpawnPointDict.ContainsKey(teamNumber))
        {
            foreach (TeamSpawnPoint sp in TeamSpawnPointDict[teamNumber])
            {
                candidates.Add(sp);
            }
        }

        results = GetNoConflictPoints(candidates, notValidPoints);

        if (results.Count == 0)
        {
            if (TeamSpawnPointDict.ContainsKey(TeamNumber.AnyTeam))
            {
                foreach (TeamSpawnPoint sp in TeamSpawnPointDict[TeamNumber.AnyTeam])
                {
                    candidates.Add(sp);
                    candidates_any.Add(sp);
                }
            }

            results = GetNoConflictPoints(candidates_any, notValidPoints);
        }

        if (results.Count > 0)
        {
            int randomIndex = Random.Range(0, results.Count);
            results[randomIndex].Spawn(playerNumber,teamNumber);
        }
        else
        {
            int randomIndex = Random.Range(0, candidates.Count);
            candidates[randomIndex].Spawn(playerNumber, teamNumber);
        }
    }

    private List<IRevivePlayer> GetNoConflictPoints(List<TeamSpawnPoint> spawnPoints, List<Vector3> notValidPoints)
    {
        List<IRevivePlayer> noConflictPlaces = new List<IRevivePlayer>();
        foreach (TeamSpawnPoint psp in spawnPoints)
        {
            bool hasConflicts = false;
            foreach (Vector3 notValidPoint in notValidPoints)
            {
                Vector3 xz_pos = psp.transform.position;
                xz_pos.y = 0;

                Vector3 xz_pos_notValidPoint = notValidPoint;
                xz_pos_notValidPoint.y = 0;

                if ((xz_pos - xz_pos_notValidPoint).magnitude < MinDistance)
                {
                    hasConflicts = true;
                }
            }

            if (!hasConflicts)
            {
                noConflictPlaces.Add(psp);
            }
        }

        return noConflictPlaces;
    }
}