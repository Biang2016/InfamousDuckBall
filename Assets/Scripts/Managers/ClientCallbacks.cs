using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Client, "MainScene")]
public class ClientCallbacks : Bolt.GlobalEventListener
{
    public override void Connected(BoltConnection connection)
    {
    }

    public override void ControlOfEntityGained(BoltEntity entity)
    {
        PlayerObjectRegistry.MyPlayer = entity.GetComponent<Player>();
    }

    public override void OnEvent(BallEvent evnt)
    {
        if (GameManager.Cur_BattleManager.Ball == null)
        {
            GameManager.Cur_BattleManager.Ball = FindObjectOfType<Ball>();
            GameManager.Cur_BattleManager.Ball.Collider.enabled = false;
            GameManager.Cur_BattleManager.Ball.RigidBody.useGravity = false;
            GameManager.Cur_BattleManager.BallDefaultPos = GameManager.Cur_BattleManager.Ball.transform.position;
        }
    }
}