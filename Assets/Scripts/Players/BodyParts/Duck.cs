using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour
{
    public Player Player;
    public DuckConfig DuckConfig => Player.DuckConfig;
    public Goalie Goalie => Player.Goalie;
    public Feet Feet;
    public Body Body;
    public Neck Neck;
    public Head Head;
    public Wings Wings;

    public Rigidbody DuckRigidbody;

    public Vector3 GetHeadPosition => Head.transform.position;

    public void Attached()
    {
        Feet.Attached();
        Body.Attached();
        Neck.Attached();
        Head.Attached();
    }

    public void Initialize()
    {
        // Feet.Initialize();
        // Body.Initialize();
        // Neck.Initialize();
        Head.Initialize();
    }

    void FixedUpdate()
    {
        if (DuckRigidbody.velocity.magnitude > DuckConfig.MaxSpeed)
        {
            DuckRigidbody.velocity = DuckRigidbody.velocity.normalized * DuckConfig.MaxSpeed;
        }
    }
}