using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 centerPoint; // The point to rotate around
    public float radius = 5f;                  // Distance from the center
    public float speed = 50f;                  // Degrees per second
    public Vector3 rotationAxis = Vector3.up;  // Default rotation around Y-axis

    private float angle = 0f;

    void Start()
    {
        // Set the initial position
        centerPoint = transform.parent.position;
    }

    void Update()
    {
        // Increase the angle over time based on speed
        angle += speed * Time.deltaTime;

        // Keep angle within 0–360 for stability
        if (angle >= 360f)
            angle -= 360f;

        // Calculate new position in a circular path
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad) * radius, 0, Mathf.Sin(rad) * radius);

        // Apply the position relative to centerPoint
        transform.position = centerPoint + offset;

        // (Optional) Make the object face forward along the circular path
        transform.LookAt(centerPoint);
    }
}
