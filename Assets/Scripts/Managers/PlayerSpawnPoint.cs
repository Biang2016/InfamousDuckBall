using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour, IRevivePlayer
{
    internal bool ConsiderInCamera = true;
    [SerializeField] private PlayerNumber allowedPlayerNumber;
    public PlayerNumber AllowedPlayerNumber => allowedPlayerNumber;

    public float PlayerReviveInterval => playerReviveInterval;
    [SerializeField] private float playerReviveInterval;

    private Dictionary<PlayerNumber, float> PlayerReviveTimeDict = new Dictionary<PlayerNumber, float>();

    public void Init()
    {
        PlayerReviveTimeDict.Clear();
        IsSpawning = false;
    }

    public void AddRevivePlayer(PlayerNumber playerNumber, float delay)
    {
        if (delay.Equals(0))
        {
            Spawn(playerNumber);
        }
        else
        {
            if (!PlayerReviveTimeDict.ContainsKey(playerNumber))
            {
                PlayerReviveTimeDict.Add(playerNumber, delay);
            }
        }
    }

    private bool IsSpawning = false;

    void Update()
    {
        if (!IsSpawning)
        {
            foreach (PlayerNumber pn in PlayerReviveTimeDict.Keys.ToList())
            {
                PlayerReviveTimeDict[pn] -= Time.deltaTime;
                if (PlayerReviveTimeDict[pn] < 0)
                {
                    PlayerReviveTimeDict.Remove(pn);
                    StartCoroutine(Co_SpawnInterval(pn));
                    break;
                }
            }
        }
    }

    private void Spawn(PlayerNumber pn)
    {
        IsSpawning = true;
        Player player = GameManager.Instance.PlayerDict[pn];
        player.SetPlayerPosition(transform.position);
        player.Reviving(ConsiderInCamera);
        IsSpawning = false;
    }

    IEnumerator Co_SpawnInterval(PlayerNumber pn)
    {
        IsSpawning = true;
        Player player = GameManager.Instance.PlayerDict[pn];
        player.SetPlayerPosition(transform.position);
        player.Reviving(ConsiderInCamera);
        yield return new WaitForSeconds(PlayerReviveInterval);
        IsSpawning = false;
    }
}