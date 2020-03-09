using UnityEngine;
using System.Collections;

public class Wings : GooseBodyPart
{
    [SerializeField] private Animator Anim;

    public void Fly()
    {
        Anim.SetTrigger("Fly");
    }

    protected override void Operate_Manual(PlayerNumber controllerIndex)
    {
    }

    protected override void Operate_AI()
    {
    }
}
