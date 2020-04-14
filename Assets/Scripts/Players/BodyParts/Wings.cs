using UnityEngine;
using System.Collections;

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
        Anim.SetBool("HasRing", true);
    }

    public void LoseRing()
    {
        Anim.SetTrigger("LoseRing");
        Anim.SetBool("HasRing", false);
    }
}