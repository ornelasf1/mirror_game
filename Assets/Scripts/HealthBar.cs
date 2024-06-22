using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameOver gameOver;

    public void Start() {
        SetMaxHealth(100);
    }

    public void Update() {
        if (!GameManager.Instance.IsGameActive()) {
            return;
        }
        CheckHealthBarEmpty();
    }

    private void CheckHealthBarEmpty() {
        if (slider.value <= 0f) {
            gameOver.ShowGameOver();
        }
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void DealDamage(float health)
    {
        slider.value -= health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
