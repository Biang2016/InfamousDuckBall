using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ScoreRingSingleSpawner : MonoBehaviour
{
    private List<ScoreRingSingle> ScoreRingSingles = new List<ScoreRingSingle>();
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
            ScoreRingSingles.Add(srs);
            return srs;
        }
        else
        {
            return null;
        }
    }

    public void Clear()
    {
        if (BoltNetwork.IsServer)
        {
            foreach (ScoreRingSingle srs in ScoreRingSingles)
            {
                if (srs != null)
                {
                    BoltNetwork.Destroy(srs.gameObject);
                }
            }
        }
    }
}