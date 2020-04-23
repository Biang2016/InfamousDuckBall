using UnityEngine;

public class FmodSFX : MonoBehaviour
{
    // Start is called before the first frame update

    [FMODUnity.EventRef] public string SFX;
    public float CollisionVelocity;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > CollisionVelocity)
        {
            //AudioWOC.Instance.PlaySound(SFX, gameObject);
        }
    }
}