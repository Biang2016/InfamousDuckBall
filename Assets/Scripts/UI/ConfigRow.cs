using UnityEngine;
using UnityEngine.UI;

public class ConfigRow : PoolObject
{
    [SerializeField] private Text Label;
    [SerializeField] private Slider Slider;

    private ConfigRowType myConfigRowType;

    public void Initialize(ConfigRowType configRowType, float min, float max)
    {
        myConfigRowType = configRowType;
        Slider.maxValue = max;
        Slider.minValue = min;
        if (BoltNetwork.IsServer)
        {
            Slider.onValueChanged.RemoveAllListeners();
            Label.text = configRowType + " = " + Slider.value.ToString("#0.00");
            switch (configRowType)
            {
                case ConfigRowType.NeckMaxLength:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.NeckMaxLengthMulti;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.NeckMaxLengthMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.MoveSpeed:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.MoveSpeedMulti;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.MoveSpeedMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.NeckSpeed:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.NeckSpeedMulti;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.NeckSpeedMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.PullRadius:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.PushRadius:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.BallBounce:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.BallBounce;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.BallBounce = Slider.value; });
                    break;
                }
                case ConfigRowType.BallWeight:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.BallWeight;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.BallWeight = Slider.value; });
                    break;
                }
                case ConfigRowType.RingDropMin:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMin;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMin = Slider.value; });
                    break;
                }
                case ConfigRowType.RingDropMax:
                {
                    Slider.value = GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMax;
                    Slider.onValueChanged.AddListener(delegate { GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMax = Slider.value; });
                    break;
                }
            }
        }
        else
        {
            Slider.interactable = false;
        }
    }

    public void Refresh()
    {
        if (BoltNetwork.IsClient)
        {
            if (GameManager.Instance.GameState)
            {
                switch (myConfigRowType)
                {
                    case ConfigRowType.NeckMaxLength:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.NeckMaxLengthMulti;
                        break;
                    }
                    case ConfigRowType.MoveSpeed:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.MoveSpeedMulti;
                        break;
                    }
                    case ConfigRowType.NeckSpeed:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.NeckSpeedMulti;
                        break;
                    }
                    case ConfigRowType.PullRadius:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.PullRadiusMulti;
                        break;
                    }
                    case ConfigRowType.PushRadius:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.PushRadiusMulti;
                        break;
                    }
                    case ConfigRowType.BallBounce:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.BallBounce;
                        break;
                    }
                    case ConfigRowType.BallWeight:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.BallWeight;
                        break;
                    }
                    case ConfigRowType.RingDropMin:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMin;
                        break;
                    }
                    case ConfigRowType.RingDropMax:
                    {
                        Slider.value = GameManager.Instance.GameState.state.DuckConfig.RingDropIntervalRandomMax;
                        break;
                    }
                }
            }
        }

        Label.text = myConfigRowType + " = " + Slider.value.ToString("#0.00");
    }

    public enum ConfigRowType
    {
        NeckMaxLength,
        MoveSpeed,
        NeckSpeed,
        PullRadius,
        PushRadius,
        BallBounce,
        BallWeight,
        RingDropMin,
        RingDropMax,
    }
}