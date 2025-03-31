using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text btnOne;
    [SerializeField] private TMP_Text btnTwo;

    public void Setup(PowerupData powerupOne, PowerupData powerupTwo)
    {
        gameObject.SetActive(true);

        CreateButtonText(btnOne, powerupOne);
        CreateButtonText(btnTwo, powerupTwo);
    }

    private static void CreateButtonText(TMP_Text buttonText, PowerupData powerup)
    {
        buttonText.text = "+" + Mathf.Abs(powerup.Positive.Value) + " " + powerup.Positive.Parameter + "\n" +
                          "-" + Mathf.Abs(powerup.Negative.Value) + " " + powerup.Negative.Parameter + "\n";
    }
}