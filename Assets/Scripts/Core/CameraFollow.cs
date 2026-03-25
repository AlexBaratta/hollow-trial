using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public float maxLagDistance = 0.5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Clamp so the camera never falls more than maxLagDistance behind the player
        Vector3 diff = desiredPosition - smoothed;
        float lagXY = new Vector2(diff.x, diff.y).magnitude;
        if (lagXY > maxLagDistance)
        {
            float scale = maxLagDistance / lagXY;
            smoothed.x = desiredPosition.x - diff.x * scale;
            smoothed.y = desiredPosition.y - diff.y * scale;
        }

        transform.position = smoothed;
    }
}