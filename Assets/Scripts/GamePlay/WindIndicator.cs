using UnityEngine;

public class WindIndicator : MonoBehaviour
{
    public Animator Anim;

    void Awake()
    {
        Anim.SetFloat("Delay", Random.Range(0f, 1f));
    }
}