using UnityEngine;
using UnityEngine.UI;

public class RingSlider : CameraFaceSlider
{
    [SerializeField] private Image FillImage;

    public override void RefreshValue(float value)
    {
        FillImage.fillAmount = value;
    }

    public override void SetColor(Color color)
    {
        FillImage.color = color;
    }
}