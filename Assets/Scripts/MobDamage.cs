using UnityEngine;

public class MobDamage : MonoBehaviour
{

    public int damageAmount;
    public float knockbackForce = 10f;
    public PlayerHealth playerHealth;


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                playerController.Knockback(knockbackDir * knockbackForce);
            }
        }
    }
}
