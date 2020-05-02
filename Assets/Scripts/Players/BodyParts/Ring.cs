using System.Collections;
using UnityEngine;

public class Ring : ScoreRing
{
    internal Duck Duck;
    internal Player Player => Duck.Player;
    internal DuckConfig DuckConfig => Duck.DuckConfig;

    public void Attached()
    {
        Duck = GetComponentInParent<Duck>();
    }

    [SerializeField] private Animator Anim;

    public void Update()
    {
        Anim.SetBool("IsCharging", Duck.Head.HeadStatus == Head.HeadStatusTypes.PushCharging);
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
        Anim.ResetTrigger("Hit");
        Anim.ResetTrigger("LoseRing");
        StartCoroutine(Co_GetRing());
    }

    public void LoseRing()
    {
        Anim.SetTrigger("LoseRing");
        Anim.ResetTrigger("GetRing");
        Anim.SetBool("HasRing", false);
    }

    IEnumerator Co_GetRing()
    {
        yield return new WaitForSeconds(1.5f);
        Duck.Player.Goalie.IsGoalie = true;
        Anim.SetBool("HasRing", true);
    }
}