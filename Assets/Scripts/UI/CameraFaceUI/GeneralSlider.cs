using UnityEngine;
using UnityEngine.UI;

public class GeneralSlider : CameraFaceSlider
{
    [SerializeField] private Slider Slider;
    [SerializeField] private Image Fill;

    public override void RefreshValue(float value)
    {
        Slider.value = value;
    }

    public override void SetColor(Color color)
    {
        Fill.color = color;
    }
}