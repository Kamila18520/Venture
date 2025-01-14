using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUpdater : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        playerValues.StartResetValues();
        slider.maxValue = playerValues.maxValue;
    }
    private void Update()
    {
        slider.value = playerValues.currentValue;
    }
}
