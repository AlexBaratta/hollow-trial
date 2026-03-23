using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void Start()
    {
        var healthBar = playerHealth.GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.Bind(playerHealth);
        }

        playerHealth.OnDied += GameOver;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }
}
