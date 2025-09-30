using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    
    public float wobbleDuration = 1.0f;        // How long the wobble lasts
    public float wobbleFrequency = 8.0f;       // Oscillations per second
    public float wobbleAmplitude = 0.25f;      // Initial stretch amount (e.g. 0.25 = 25%)

    private Vector3 originalScale;
    private float elapsed = 0f;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()            // runs when instantiated
    {
        elapsed = 0f;
    }

    void Update()
    {
        if (elapsed < wobbleDuration)
        {
            elapsed += Time.deltaTime;

            // Damping factor from 1 → 0 over time
            float damping = 1f - (elapsed / wobbleDuration);
            damping = Mathf.Clamp01(damping);

            // Oscillate using a sine wave
            float sine = Mathf.Sin(elapsed * wobbleFrequency * Mathf.PI * 2f);

            // Compute scale factors
            float scaleY = 1f + sine * wobbleAmplitude * damping;
            float scaleX = 1f - sine * (wobbleAmplitude * 0.5f) * damping;
            float scaleZ = scaleX;

            transform.localScale = new Vector3(
                originalScale.x * scaleX,
                originalScale.y * scaleY,
                originalScale.z * scaleZ
            );
        }
        else
        {
            // Restore the original scale once finished
            transform.localScale = originalScale;
        }
    }
}