using UnityEngine;

public abstract class CameraFaceSlider : CameraFaceUI
{
    public abstract void RefreshValue(float value);

    public abstract void SetColor(Color color);
}