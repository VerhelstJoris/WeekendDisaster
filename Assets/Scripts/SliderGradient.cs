using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderGradient : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;

    public Image SliderFill;

    // Update is called once per frame
    void Update()
    {
        float val = Slider.value;
        SliderFill.color = Gradient.Evaluate(val);

    }
}
