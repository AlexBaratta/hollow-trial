using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 3f;
    public int damage = 1;

    private Vector2 direction;
    private float timer;
    private ObjectShooter shooter;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void Launch(Vector2 dir, ObjectShooter owner)
    {
        direction = dir.normalized;
        timer = lifespan;
        shooter = owner;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Return();
            return;
        }

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        MobHealth mobHealth = other.GetComponent<MobHealth>();
        if (mobHealth != null)
        {
            mobHealth.TakeDamage(damage);
        }

        Return();
    }

    void Return()
    {
        if (shooter != null)
        {
            shooter.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
