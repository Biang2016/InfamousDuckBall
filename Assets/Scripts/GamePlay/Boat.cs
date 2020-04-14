using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public void MouthOpen()
    {
        Anim.SetTrigger("Open");
    }

    public void MouthClose()
    {
        Anim.SetTrigger("Close");
    }
}