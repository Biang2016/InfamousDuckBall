using System.Collections.Generic;

public class ConfigManager : MonoSingleton<ConfigManager>
{
    public static SortedDictionary<BattleTypes, int> BattleMaxPlayerNumberDict = new SortedDictionary<BattleTypes, int>
    {
        {BattleTypes.Smash, 2},
        {BattleTypes.FlagRace, 4},
    };

    public const int MaxPlayerNumber_Local = 4;

    public const int Smash_TeamStartScore = 5;
    public const int Smash_TeamTargetMegaScore = 2;

    public const int TeamNumberCount = 4;
    public const int CostumeTypeCount = 3;

    public float RingRecoverTime = 3f;

    public const int FlagRace_TeamTargetScore = 10;

    public float BallWeight = 1f;
    public float BallBounce = 0.5f;
    public float RingDropIntervalRandomMin = 3f;
    public float RingDropIntervalRandomMax = 10f;

    // Multiplier
    private float NeckMaxLengthMulti = 1f;
    private float MoveSpeedMulti = 1f;
    private float NeckSpeedMulti = 1f;
    private float PullRadiusMulti = 1f;
    private float PushRadiusMulti = 1f;
    private float BallBounceMulti = 1f;
    private float BallWeightMulti = 1f;
    private float RingDropIntervalRandomMinMulti = 1f;
    private float RingDropIntervalRandomMaxMulti = 1f;

    public DuckConfiguration_GetterSetter DuckConfiguration_Multiplier = new DuckConfiguration_GetterSetter();

    public class DuckConfiguration_GetterSetter
    {
        public float NeckMaxLengthMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.NeckMaxLengthMulti;
                }
                else
                {
                    return Instance.NeckMaxLengthMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.NeckMaxLengthMulti = value;
                }
                else
                {
                    Instance.NeckMaxLengthMulti = value;
                }
            }
        }

        public float MoveSpeedMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.MoveSpeedMulti;
                }
                else
                {
                    return Instance.MoveSpeedMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.MoveSpeedMulti = value;
                }
                else
                {
                    Instance.MoveSpeedMulti = value;
                }
            }
        }

        public float NeckSpeedMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.NeckSpeedMulti;
                }
                else
                {
                    return Instance.NeckSpeedMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.NeckSpeedMulti = value;
                }
                else
                {
                    Instance.NeckSpeedMulti = value;
                }
            }
        }

        public float PullRadiusMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti;
                }
                else
                {
                    return Instance.PullRadiusMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti = value;
                }
                else
                {
                    Instance.PullRadiusMulti = value;
                }
            }
        }

        public float PushRadiusMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti;
                }
                else
                {
                    return Instance.PushRadiusMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti = value;
                }
                else
                {
                    Instance.PushRadiusMulti = value;
                }
            }
        }

        public float BallBounceMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.BallBounceMulti;
                }
                else
                {
                    return Instance.BallBounceMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.BallBounceMulti = value;
                }
                else
                {
                    Instance.BallBounceMulti = value;
                }
            }
        }

        public float BallWeightMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.BallWeightMulti;
                }
                else
                {
                    return Instance.BallWeightMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.BallWeightMulti = value;
                }
                else
                {
                    Instance.BallWeightMulti = value;
                }
            }
        }

        public float RingDropIntervalRandomMinMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMinMulti;
                }
                else
                {
                    return Instance.RingDropIntervalRandomMinMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMinMulti = value;
                }
                else
                {
                    Instance.RingDropIntervalRandomMinMulti = value;
                }
            }
        }

        public float RingDropIntervalRandomMaxMulti
        {
            get
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    return GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMaxMulti;
                }
                else
                {
                    return Instance.RingDropIntervalRandomMaxMulti;
                }
            }
            set
            {
                if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
                {
                    GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMaxMulti = value;
                }
                else
                {
                    Instance.RingDropIntervalRandomMaxMulti = value;
                }
            }
        }
    }
}