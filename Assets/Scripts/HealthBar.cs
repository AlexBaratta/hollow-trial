using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void Bind(PlayerHealth health)
    {
        health.OnHealthChanged += UpdateBar;
        SetMaxHealth(health.maxHealth);
    }

    public void Bind(MobHealth health)
    {
        health.OnHealthChanged += UpdateBar;
        SetMaxHealth(health.maxHealth);
    }

    private void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    private void UpdateBar(int current, int max)
    {
        slider.value = current;
    }
}
