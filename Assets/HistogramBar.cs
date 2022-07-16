using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HistogramBar : MonoBehaviour
{
    public  Slider   slider;
    public  TMP_Text text;

    public void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        text   = GetComponentInChildren<TMP_Text>();
        Assert.IsNotNull(slider);
        Assert.IsNotNull(text);

        slider.value = 0;
        text.text    = "0";
    }

    public void SetValue(int number, double weight, bool showText = true)
    {
        slider.value = (float)weight;
        text.text    = number.ToString();
        text.gameObject.SetActive(showText);
    }
}
