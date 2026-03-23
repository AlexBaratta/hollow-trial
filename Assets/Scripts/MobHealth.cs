using System;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke();
        MobPool.Instance.Return(gameObject);
    }
}
