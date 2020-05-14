using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private Animator Anim;

    public Transform ScoreRingsPivot;

    public ScoreRingManager ScoreRingManager;

    public void MouthOpen()
    {
        Anim.SetTrigger("Open");
    }

    public void MouthClose()
    {
        Anim.SetTrigger("Close");
    }
}