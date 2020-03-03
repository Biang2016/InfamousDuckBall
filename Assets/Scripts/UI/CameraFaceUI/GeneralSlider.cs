using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeneralSlider : CameraFaceUI
{
    [SerializeField] private Slider Slider;

    public void RefreshValue(float value)
    {
        Slider.value = value;
    }

}
