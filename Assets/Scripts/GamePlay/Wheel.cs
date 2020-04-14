using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public void Rotate()
    {
        Anim.SetTrigger("Rotate");
    }

    public void Stop()
    {
        Anim.SetTrigger("Stop");
    }
}