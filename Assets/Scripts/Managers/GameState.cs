using Bolt;

public class GameState : EntityBehaviour<IGameState>
{
    // Use this for initialization
    void Start()
    {
        if (BoltNetwork.IsServer)
        {
            state.DuckConfig.NeckMaxLengthMulti = 1f;
            state.DuckConfig.MoveSpeedMulti = 1f;
            state.DuckConfig.NeckSpeedMulti = 1f;
            state.DuckConfig.PullRadiusMulti = 1f;
            state.DuckConfig.PushRadiusMulti = 1f;
            state.DuckConfig.BallBounce = 1f;
            state.DuckConfig.BallWeight = 1f;
            state.DuckConfig.RingDropIntervalRandomMin = 1f;
            state.DuckConfig.RingDropIntervalRandomMax = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}