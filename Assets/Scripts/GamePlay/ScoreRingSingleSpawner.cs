using System.Collections.Generic;
using UnityEngine;

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

    public void Spawn()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            if (ScoreRingSingles.Count < 5)
            {
                ScoreRingSingle srs = null;
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    BoltEntity be = BoltNetwork.Instantiate(BoltPrefabs.ScoreRingSingle, GetRandomPos(), Random.rotation);
                    srs = be.GetComponent<ScoreRingSingle>();
                }
                else
                {
                    GameObject be = Instantiate(PrefabManager.Instance.GetPrefab("ScoreRingSingle"), GetRandomPos(), Random.rotation);
                    srs = be.GetComponent<ScoreRingSingle>();
                }

                srs.TeamNumber = TeamNumber;
                srs.CostumeType = (CostumeType) Random.Range(0, ConfigManager.CostumeTypeCount);
                ScoreRingSingles.Add(srs);
                srs.OnRemove = delegate { ScoreRingSingles.Remove(srs); };
            }
        }
    }

    public void Clear()
    {
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            foreach (ScoreRingSingle srs in ScoreRingSingles)
            {
                if (srs != null)
                {
                    if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                    {
                        BoltNetwork.Destroy(srs.gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(srs.gameObject);
                    }
                }
            }
        }
    }
}