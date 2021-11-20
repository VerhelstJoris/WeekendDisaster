using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    public void SetBarValue(float fillPercentage)
    {
        slider.value = fillPercentage;
    }
}