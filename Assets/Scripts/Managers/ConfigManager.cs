using UnityEngine;
using System.Collections;

public class ConfigManager : MonoSingleton<ConfigManager>
{
    public const int TeamStartScore = 5;
    public const int MaximalPlayerNumber = 4;
    public const int TeamNumberCount = 4;
    public const int CostumeTypeCount = 3;

    public float RingRecoverTime = 3f;
}