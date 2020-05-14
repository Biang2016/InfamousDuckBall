using UnityEngine;

public class FXManager : MonoSingleton<FXManager>
{
    public FX PlayFX(FX_Type fx_Type, Vector3 from, Quaternion direction)
    {
        FX fx = GameObjectPoolManager.Instance.FXDict[fx_Type].AllocateGameObject<FX>(transform);
        fx.transform.position = from;
        fx.transform.rotation = direction;
        fx.Play();
        return fx;
    }
}

public enum FX_Type
{
    BallKickParticleSystem = 1,
    ScoreRingExplosion = 2,
}