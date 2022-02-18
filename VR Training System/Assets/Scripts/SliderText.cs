using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

// displays the value of a slider to a text
public class SliderText : MonoBehaviour
{
    public Transform sliderTrafo;

    void Start()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        GetComponent<TextMeshProUGUI>().text = "" + Math.Round(sliderTrafo.GetComponent<Slider>().value, 3);
    }
}
