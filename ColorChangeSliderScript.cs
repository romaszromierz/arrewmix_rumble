using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeSliderScript : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public Gradient gradient;

    void Update()
    {
        float normalizedValue = slider.normalizedValue;
        fillImage.color = GetGradientColor(normalizedValue);
    }

    Color GetGradientColor(float value)
    {
        return gradient.Evaluate(value);
    }
}
