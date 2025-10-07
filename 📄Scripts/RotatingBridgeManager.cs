using UnityEngine;

public class RotatingBridgeManager : MonoBehaviour
{
    [SerializeField] Transform rotatingPoint;
    [SerializeField] float rotationSpeed = 45f;      // degrees per second
    [SerializeField] PlayerMovement playerMovement;

    private float startAngle;                        // original Y angle
    private float targetAngle;                       // desired angle (90° or back to 0°)
    private bool isRotating = false;
    private bool isOpen = false;                     // whether bridge is currently at 90°

    void Start()
    {
        startAngle = transform.eulerAngles.y;
        targetAngle = startAngle;
    }

    void Update()
    {
        // Trigger to open (rotate +90°) only if not already rotating/open
        if (playerMovement.isPlayerOnBridge && !isRotating && !isOpen)
        {
            targetAngle = startAngle + 90f;
            StartCoroutine(RotateBridge(targetAngle, true));
        }

        // Trigger to close (rotate back) only if not rotating/closed
        if (!playerMovement.isPlayerOnBridge && !isRotating && isOpen)
        {
            targetAngle = startAngle;
            StartCoroutine(RotateBridge(targetAngle, false));
        }
    }

    private System.Collections.IEnumerator RotateBridge(float endAngle, bool opening)
    {
        isRotating = true;

        // Get shortest angle direction
        float currentAngle = transform.eulerAngles.y;
        float totalRotation = Mathf.DeltaAngle(currentAngle, endAngle);

        // Rotate until we reach target
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, endAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float angleToRotate = Mathf.Sign(totalRotation) * step;

            // Avoid overshooting
            if (Mathf.Abs(angleToRotate) > Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, endAngle)))
            {
                angleToRotate = Mathf.DeltaAngle(transform.eulerAngles.y, endAngle);
            }

            transform.RotateAround(rotatingPoint.position, Vector3.up, angleToRotate);
            yield return null;  // wait for next frame
        }

        // Snap to final angle to avoid tiny errors
        float y = endAngle;
        Vector3 finalEuler = transform.eulerAngles;
        finalEuler.y = y;
        transform.eulerAngles = finalEuler;

        isOpen = opening;
        isRotating = false;
    }
}
