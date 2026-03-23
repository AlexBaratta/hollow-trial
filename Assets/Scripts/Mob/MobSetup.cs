using UnityEngine;

public class MobSetup : MonoBehaviour
{
    private MobHealth health;
    private HealthBar healthBar;

    private void Awake()
    {
        health = GetComponent<MobHealth>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void OnEnable()
    {
        if (health != null && healthBar != null)
        {
            healthBar.Bind(health);
        }
    }

    private void OnDisable()
    {
        if (health != null && healthBar != null)
        {
            healthBar.Unbind(health);
        }
    }
}
