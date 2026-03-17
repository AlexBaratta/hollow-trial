using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public Transform visual; // drag your triangle child here

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.up;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movement.y += 1f;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1f;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1f;
        if (Keyboard.current.dKey.isPressed) movement.x += 1f;

        movement = movement.normalized;

        // Save last non-zero direction so the triangle keeps facing that way
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }

        // Rotate the child visual to face move direction
        if (visual != null)
        {
            float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
            visual.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse button pressed.");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}