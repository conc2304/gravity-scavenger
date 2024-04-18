using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    public Slider slider;

    public void SetSlider(float amount)
    {
        slider.value = Mathf.Lerp(slider.value, amount, 0.1f);
    }

    public void SetSliderMax(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;
    }
}
