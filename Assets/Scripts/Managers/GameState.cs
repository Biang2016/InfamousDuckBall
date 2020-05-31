using Bolt;

public class GameState : EntityBehaviour<IGameState>
{
    void Start()
    {
        if (BoltNetwork.IsServer)
        {
            state.DuckConfig.NeckMaxLengthMulti = 1f;
            state.DuckConfig.MoveSpeedMulti = 1f;
            state.DuckConfig.NeckSpeedMulti = 1f;
            state.DuckConfig.PullRadiusMulti = 1f;
            state.DuckConfig.PushRadiusMulti = 1f;
            state.DuckConfig.BallBounceMulti = 1f;
            state.DuckConfig.BallWeightMulti = 1f;
            state.DuckConfig.RingDropIntervalRandomMinMulti = 1f;
            state.DuckConfig.RingDropIntervalRandomMaxMulti = 1f;
        }
    }

    void Update()
    {
    }
}