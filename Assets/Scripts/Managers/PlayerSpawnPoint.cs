using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour, IRevivePlayer
{
    internal bool ConsiderInCamera = true;
    [SerializeField] private PlayerNumber allowedPlayerNumber;
    public PlayerNumber AllowedPlayerNumber => allowedPlayerNumber;

    public void Init()
    {
    }

    void Update()
    {
    }

    public void Spawn(PlayerInfo playerInfo)
    {
        Player player = GameManager.Cur_BattleManager.PlayerDict[(PlayerNumber) playerInfo.PlayerNumber];
        player.SetPlayerPosition(transform.position);
        player.Reviving();
    }
}