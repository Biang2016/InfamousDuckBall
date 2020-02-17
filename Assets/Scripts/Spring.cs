using UnityEngine;
using DG.Tweening;

public class Spring : MonoBehaviour
{
    [SerializeField] private JoystickButton Button = JoystickButton.None;
    [SerializeField] private Transform SpringObject;

    private Vector3 DefaultPos;
    [SerializeField] private Vector3 BumperDest;

    void Awake()
    {
        DefaultPos = SpringObject.localPosition;
    }

    void Update()
    {
        if (Button != JoystickButton.None)
        {
            if (Input.GetButtonDown(Button.ToString()))
            {
                SpringObject.DOPause();
                SpringObject.DOLocalMove(DefaultPos + BumperDest, 0.1f).OnComplete(delegate { SpringObject.DOLocalMove(DefaultPos, 0.3f); });
            }
        }
    }
}