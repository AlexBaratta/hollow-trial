using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;

    private const string WalkDownState = "WalkDown";
    private const string WalkLeftState = "WalkLeft";
    private const string WalkRightState = "WalkRight";
    private const string WalkUpState = "WalkUp";

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;
    private string currentAnimationState = WalkDownState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movement.y += 1f;
        if (Keyboard.current.sKey.isPressed) movement.y -= 1f;
        if (Keyboard.current.aKey.isPressed) movement.x -= 1f;
        if (Keyboard.current.dKey.isPressed) movement.x += 1f;

        movement = movement.normalized;

        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }

        UpdateAnimation();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse button pressed.");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void UpdateAnimation()
    {
        if (animator == null)
        {
            return;
        }

        string targetState = GetAnimationState(lastMoveDirection);

        if (currentAnimationState != targetState)
        {
            currentAnimationState = targetState;
            animator.Play(currentAnimationState, 0, 0f);
        }

        animator.speed = movement == Vector2.zero ? 0f : 1f;
    }

    string GetAnimationState(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0f ? WalkRightState : WalkLeftState;
        }

        return direction.y > 0f ? WalkUpState : WalkDownState;
    }
}
