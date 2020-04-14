using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[BoltGlobalBehaviour("MainScene")]
public class GameManager : Bolt.GlobalEventListener
{
    public const int MaximalPlayerNumber = 4;
    public const int TeamNumberCount = 4;

    internal static int LayerMask_RangeOfActivity;
    internal static int Layer_RangeOfActivity;
    internal static int Layer_PlayerCollider1;
    internal static int Layer_PlayerCollider2;
    internal static int Layer_PlayerCollider3;
    internal static int Layer_PlayerCollider4;
    internal static int Layer_BallKicker;
    internal static int Layer_Ball;
    internal SortedDictionary<PlayerNumber, int> Layer_PlayerBall = new SortedDictionary<PlayerNumber, int>();

    void Awake()
    {
        Application.targetFrameRate = 60;
        LayerMask_RangeOfActivity = LayerMask.GetMask("RangeOfActivity");
        Layer_RangeOfActivity = LayerMask.NameToLayer("RangeOfActivity");
        Layer_PlayerCollider1 = LayerMask.NameToLayer("PlayerCollider1");
        Layer_PlayerCollider2 = LayerMask.NameToLayer("PlayerCollider2");
        Layer_PlayerCollider3 = LayerMask.NameToLayer("PlayerCollider3");
        Layer_PlayerCollider4 = LayerMask.NameToLayer("PlayerCollider4");
        Layer_BallKicker = LayerMask.NameToLayer("BallKicker");
        Layer_Ball = LayerMask.NameToLayer("Ball");
        Layer_PlayerBall.Add(PlayerNumber.Player1, LayerMask.NameToLayer("Ball1"));
        Layer_PlayerBall.Add(PlayerNumber.Player2, LayerMask.NameToLayer("Ball2"));
        Layer_PlayerBall.Add(PlayerNumber.Player3, LayerMask.NameToLayer("Ball3"));
        Layer_PlayerBall.Add(PlayerNumber.Player4, LayerMask.NameToLayer("Ball4"));
    }

    public bool IsPlayerColliderLayer(int layerIndex)
    {
        return layerIndex == Layer_PlayerCollider1 || layerIndex == Layer_PlayerCollider2 || layerIndex == Layer_PlayerCollider3 || layerIndex == Layer_PlayerCollider4;
    }

    public bool IsBallLayer(int layerIndex)
    {
        return layerIndex == Layer_Ball || Layer_PlayerBall.Values.ToList().Contains(layerIndex);
    }

    void Start()
    {
    }

    public override void SceneLoadLocalDone(string scene)
    {
        SwitchBattle();
        Input.ResetInputAxes();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public static BattleManager Cur_BattleManager;

    public void SwitchBattle()
    {
        GameObject battle_prefab = PrefabManager.Instance.GetPrefab("Battle_PVP4");
        GameObject battle_go = Instantiate(battle_prefab);
        BattleManager battleManager = battle_go.GetComponent<BattleManager>();

        Cur_BattleManager = battleManager;
        Cur_BattleManager.Initialize();
    }

    public override void OnEvent(BattleStartEvent evnt)
    {
        Cur_BattleManager.IsStart = true;
    }
}