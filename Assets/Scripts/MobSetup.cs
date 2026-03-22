using UnityEngine;

public class MobSetup : MonoBehaviour
{
    void Start()
    {
        var health = GetComponent<MobHealth>();
        var healthBar = GetComponentInChildren<HealthBar>();

        if (health != null && healthBar != null)
        {
            healthBar.Bind(health);
        }
    }
}
