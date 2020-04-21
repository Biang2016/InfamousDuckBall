using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ScoreRingSingleSpawner : MonoBehaviour
{
    private List<Transform> Points = new List<Transform>();
    public TeamNumber TeamNumber;

    void Awake()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            Points.Add(t);
        }
    }

    public Vector3 GetRandomPos()
    {
        return Points[Random.Range(0, Points.Count)].position;
    }

    public ScoreRingSingle Spawn()
    {
        if (BoltNetwork.IsServer)
        {
            BoltEntity be = BoltNetwork.Instantiate(BoltPrefabs.ScoreRingSingle, GetRandomPos(), Random.rotation);
            ScoreRingSingle srs = be.GetComponent<ScoreRingSingle>();
            srs.state.TeamNumber = (int) TeamNumber;
            srs.state.CostumeType = Random.Range(0, ConfigManager.CostumeTypeCount);

            return srs;
        }
        else
        {
            return null;
        }
    }
}