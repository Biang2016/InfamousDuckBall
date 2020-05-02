using UnityEngine;

public class Duck : MonoBehaviour
{
    public Player Player;
    public DuckConfig DuckConfig => Player.DuckConfig;
    public Feet Feet;
    public Body Body;
    public Neck Neck;
    public Head Head;
    public Wings Wings;
    public Ring Ring;
    public SunGlasses SunGlasses;
    public PlayerCostume PlayerCostume => Player.PlayerCostume;

    public Rigidbody DuckRigidbody;

    public Vector3 GetHeadPosition => Head.transform.position;

    public void Attached()
    {
        Feet.Attached();
        Body.Attached();
        Neck.Attached();
        Head.Attached();
        Ring.Attached();
        SunGlasses.Attached();
        PlayerCostume.Attached();
    }

    public void Detached()
    {
        Ring.StopAllCoroutines();
        Body.StopAllCoroutines();
        StopAllCoroutines();
    }

    public void Initialize()
    {
        Head.Initialize();
        Feet.ReleaseChargingCircle();
    }

    void FixedUpdate()
    {
        if (DuckRigidbody.velocity.magnitude > DuckConfig.MaxSpeed)
        {
            DuckRigidbody.velocity = DuckRigidbody.velocity.normalized * DuckConfig.MaxSpeed;
        }
    }
}