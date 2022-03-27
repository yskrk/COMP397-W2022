using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIController : MonoBehaviour
{
    [SerializeField] private Slider HealthBar;
    [SerializeField] TMP_Text HealthBarValue;

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Scene");
    }

    public void TakeDamage(int damage)
    {
        int health = Int32.Parse(HealthBar.value.ToString());
        health -= damage;
        HealthBar.value = health;
    }

    public void OnHealthBarChanged()
    {

        HealthBarValue.text = HealthBar.value.ToString();
    }
}
