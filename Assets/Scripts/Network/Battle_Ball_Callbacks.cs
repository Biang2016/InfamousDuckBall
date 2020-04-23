using UnityEngine;

[BoltGlobalBehaviour("Battle_FlagRace", "Battle_Smash")]
public class Battle_Ball_Callbacks : Bolt.GlobalEventListener
{
    void Awake()
    {
    }

    public override void Connected(BoltConnection connection)
    {
    }

    public override void SceneLoadLocalDone(string map)
    {
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
    }

    public override void OnEvent(BallEvent evnt)
    {
        if (evnt.BallName == "SmashBall")
        {
            Ball ball = evnt.BallEntity.GetComponent<Ball>();
            ((BattleManager_Smash) GameManager.Instance.Cur_BattleManager).Ball = ball;
        }

        if (evnt.BallName == "FlagRaceBall_Left")
        {
            Ball ball = evnt.BallEntity.GetComponent<Ball>();
            ((BattleManager_FlagRace) GameManager.Instance.Cur_BattleManager).LeftBall = ball;
        }

        if (evnt.BallName == "FlagRaceBall_Right")
        {
            Ball ball = evnt.BallEntity.GetComponent<Ball>();
            ((BattleManager_FlagRace) GameManager.Instance.Cur_BattleManager).RightBall = ball;
        }
    }
}