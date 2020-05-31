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
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Local || BoltNetwork.IsServer)
        {
            Slider.onValueChanged.RemoveAllListeners();
            Label.text = configRowType + " = " + Slider.value.ToString("#0.00");
            switch (configRowType)
            {
                case ConfigRowType.NeckMaxLength:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.NeckMaxLengthMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.NeckMaxLengthMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.MoveSpeed:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.MoveSpeedMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.MoveSpeedMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.NeckSpeed:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.NeckSpeedMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.NeckSpeedMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.PullRadius:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.PullRadiusMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.PullRadiusMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.PushRadius:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.PushRadiusMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.PushRadiusMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.BallBounce:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.BallBounceMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.BallBounceMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.BallWeight:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.BallWeightMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.BallWeightMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.RingDropMin:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMinMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMinMulti = Slider.value; });
                    break;
                }
                case ConfigRowType.RingDropMax:
                {
                    Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMaxMulti;
                    Slider.onValueChanged.AddListener(delegate { ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMaxMulti = Slider.value; });
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
        if (GameManager.Instance.M_NetworkMode == GameManager.NetworkMode.Online)
        {
            if (BoltNetwork.IsClient)
            {
                if (GameManager.Instance.GameState)
                {
                    RefreshCore();
                }
            }
        }
        else
        {
            RefreshCore();
        }

        Label.text = myConfigRowType + " = " + Slider.value.ToString("#0.00");
    }

    private void RefreshCore()
    {
        switch (myConfigRowType)
        {
            case ConfigRowType.NeckMaxLength:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.NeckMaxLengthMulti;
                break;
            }
            case ConfigRowType.MoveSpeed:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.MoveSpeedMulti;
                break;
            }
            case ConfigRowType.NeckSpeed:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.NeckSpeedMulti;
                break;
            }
            case ConfigRowType.PullRadius:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.PullRadiusMulti;
                break;
            }
            case ConfigRowType.PushRadius:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.PushRadiusMulti;
                break;
            }
            case ConfigRowType.BallBounce:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.BallBounceMulti;
                break;
            }
            case ConfigRowType.BallWeight:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.BallWeightMulti;
                break;
            }
            case ConfigRowType.RingDropMin:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMinMulti;
                break;
            }
            case ConfigRowType.RingDropMax:
            {
                Slider.value = ConfigManager.Instance.DuckConfiguration_Multiplier.RingDropIntervalRandomMaxMulti;
                break;
            }
        }
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