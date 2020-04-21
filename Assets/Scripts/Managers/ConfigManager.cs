using UnityEngine;
using System.Collections;

public class ConfigManager : MonoSingleton<ConfigManager>
{
    public const int Smash_TeamStartScore = 5;
    public const int Smash_TeamTargetMegaScore = 2;
    public const int MaximalPlayerNumber = 4;
    public const int TeamNumberCount = 4;
    public const int CostumeTypeCount = 3;

    public float RingRecoverTime = 3f;

    public const int FlagRace_TeamTargetScore = 10;

    public float BallWeight = 1f;
    public float BallBounce = 0.5f;
    public float RingDropIntervalRandomMin = 3f;
    public float RingDropIntervalRandomMax = 10f;
}