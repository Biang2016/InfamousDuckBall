using UnityEngine;

public class Wings : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public void Hit()
    {
        Anim.SetTrigger("Hit");
    }

    public void Kick()
    {
        Anim.SetTrigger("Kick");
    }

    public void Charge()
    {
        Anim.SetTrigger("Charge");
    }

    public void Walking()
    {
        Anim.SetBool("Walking", true);
    }

    public void NotWalking()
    {
        Anim.SetBool("Walking", false);
    }

    public void GetRing()
    {
        Anim.SetTrigger("GetRing");
        Anim.ResetTrigger("LoseRing");
        Anim.SetBool("HasRing", true);
    }

    public void LoseRing()
    {
        Anim.SetTrigger("LoseRing");
        Anim.ResetTrigger("GetRing");
        Anim.SetBool("HasRing", false);
    }
}