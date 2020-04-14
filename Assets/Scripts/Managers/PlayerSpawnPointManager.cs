using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 自行寻找合适复活点
/// </summary>
public class PlayerSpawnPointManager : MonoBehaviour
{
    private Dictionary<PlayerNumber, List<PlayerSpawnPoint>> PlayerSpawnPointDict = new Dictionary<PlayerNumber, List<PlayerSpawnPoint>>();

    [SerializeField] private bool ConsiderInCamera = true;

    [SerializeField] private float MinDistance = 5;

    void Awake()
    {
        List<PlayerSpawnPoint> PlayerSpawnPoints = gameObject.GetComponentsInChildren<PlayerSpawnPoint>(true).ToList();
        foreach (PlayerSpawnPoint sp in PlayerSpawnPoints)
        {
            sp.ConsiderInCamera = ConsiderInCamera;
            if (!PlayerSpawnPointDict.ContainsKey(sp.AllowedPlayerNumber))
            {
                PlayerSpawnPointDict.Add(sp.AllowedPlayerNumber, new List<PlayerSpawnPoint>());
            }

            PlayerSpawnPointDict[sp.AllowedPlayerNumber].Add(sp);
        }
    }

    public void Init()
    {
    }

    void Update()
    {
    }

    public void Spawn(PlayerNumber playerNumber)
    {
        List<Vector3> notValidPoints = GameManager.Cur_BattleManager.GetAllPlayerPositions();

        List<PlayerSpawnPoint> candidates = new List<PlayerSpawnPoint>();
        List<PlayerSpawnPoint> candidates_any = new List<PlayerSpawnPoint>();
        List<IRevivePlayer> results = new List<IRevivePlayer>();

        if (PlayerSpawnPointDict.ContainsKey(playerNumber))
        {
            foreach (PlayerSpawnPoint sp in PlayerSpawnPointDict[playerNumber])
            {
                candidates.Add(sp);
            }
        }

        results = GetNoConflictPoints(candidates, notValidPoints);

        if (results.Count == 0)
        {
            if (PlayerSpawnPointDict.ContainsKey(PlayerNumber.AnyPlayer))
            {
                foreach (PlayerSpawnPoint sp in PlayerSpawnPointDict[PlayerNumber.AnyPlayer])
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
            results[randomIndex].Spawn(playerNumber);
        }
        else
        {
            int randomIndex = Random.Range(0, candidates.Count);
            candidates[randomIndex].Spawn(playerNumber);
        }
    }

    private List<IRevivePlayer> GetNoConflictPoints(List<PlayerSpawnPoint> spawnPoints, List<Vector3> notValidPoints)
    {
        List<IRevivePlayer> noConflictPlaces = new List<IRevivePlayer>();
        foreach (PlayerSpawnPoint psp in spawnPoints)
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