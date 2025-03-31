using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private Slider _slider;
    private Slide _slideAbility;

    private void Awake()
    {
        _slideAbility = GameManager.Instance.GetPlayer().GetComponent<Slide>();
        _slider = GetComponent<Slider>();
    }

    private void SetEnergy(float value)
    {
        _slider.value = value;
    }

    private void Update()
    {
        SetEnergy(_slideAbility.GetSliderEnergy());
    }
}