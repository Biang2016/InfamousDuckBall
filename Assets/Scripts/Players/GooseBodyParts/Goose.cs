using UnityEngine;
using System.Collections;

public class Goose : MonoBehaviour
{
    public Feet Feet;
    public Body Body;
    public Neck Neck;
    public Head Head;
    public Wings Wings;

    public Vector3 GetHeadPosition => Head.transform.position;

}