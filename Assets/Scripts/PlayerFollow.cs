using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float separationRadius = 1.5f;
    public float separationWeight = 1f;
    public Transform target;
    Rigidbody2D rb;

    Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target)
        {
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;

            Vector2 separation = Vector2.zero;
            Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);
            foreach (var col in nearby)
            {
                if (col.gameObject != gameObject && col.GetComponent<PlayerFollow>() != null)
                {
                    Vector2 away = (Vector2)transform.position - (Vector2)col.transform.position;
                    float dist = away.magnitude;
                    if (dist > 0f)
                        separation += away.normalized / dist;
                }
            }

            moveDirection = (direction + separation * separationWeight).normalized;
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

}
