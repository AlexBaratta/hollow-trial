using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifespan = 3f;
    public int damage = 1;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile hit: " + other.gameObject.name + " (tag: " + other.tag + ")");

        if (other.CompareTag("Player"))
            return;

        MobHealth mobHealth = other.GetComponent<MobHealth>();
        if (mobHealth != null)
        {
            mobHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
