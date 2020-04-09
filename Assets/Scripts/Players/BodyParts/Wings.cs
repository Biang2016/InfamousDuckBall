using UnityEngine;
using System.Collections;

public class Wings : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public void Fly()
    {
        Anim.SetTrigger("Fly");
    }
}