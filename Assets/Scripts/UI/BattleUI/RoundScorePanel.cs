using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundScorePanel : BaseUIForm
{
    [SerializeField] private Image BlueScoreImage;
    [SerializeField] private Image RedScoreImage;
    [SerializeField] private Sprite[] BluePointSprites;
    [SerializeField] private Sprite[] RedPointSprites;

    [SerializeField] private Animator Anim;

    private int BlueScore;
    private int RedScore;

    public void Initialize(int blueScore, int redScore)
    {
        BlueScore = blueScore;
        RedScore = redScore;
    }

    public void ShowJump()
    {
        Anim.SetTrigger("Jump");
    }

    public void OnBlueTextReady()
    {
        BlueScoreImage.sprite = BluePointSprites[BlueScore];
    }

    public void OnRedTextReady()
    {
        RedScoreImage.sprite = RedPointSprites[RedScore];
    }

    public override void Hide()
    {
        base.Hide();
        if (PlayerObjectRegistry.MyPlayer && PlayerObjectRegistry.MyPlayer.PlayerController.Controller != null)
        {
            PlayerObjectRegistry.MyPlayer.Controller.Active = true;
        }
    }
}