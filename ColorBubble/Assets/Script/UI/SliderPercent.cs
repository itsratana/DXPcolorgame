using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderPercent : MonoBehaviour
{
    [SerializeField] Text percentText;
    [SerializeField] Slider slider;
    void Start()
    {
        slider.onValueChanged.AddListener((value) =>
        {
            UpdateText();
        });
    }
    public void UpdateText()
    {
        float maxValue = slider.maxValue;
        float percentage = (slider.value / maxValue) * 10f;
        percentText.text = percentage.ToString("0.00");
    }

}
