using UnityEngine;

public class BattleManager_Prepare : BattleManager
{
    public Boat Boat;

    public override void Child_Initialize()
    {
        if (BoltNetwork.IsServer)
        {
            BoltEntity be1 = BoltNetwork.Instantiate(BoltPrefabs.ScoreRings, Boat.ScoreRingsPivot.position, Boat.ScoreRingsPivot.rotation);
            Boat.ScoreRingManager = be1.GetComponent<ScoreRingManager>();
        }

        GameManager.Instance.DebugPanel.Display();
        PlayerControllerMoveDirectionQuaternion = Quaternion.Euler(0, 0, 0);
        ResetAllPlayers();
        GameManager.Instance.DebugPanel.ConfigRows.Initialize();
    }

    protected override void Update()
    {
        base.Update();
    }
}