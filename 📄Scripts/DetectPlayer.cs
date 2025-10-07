using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public bool isPlayerDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerDetected = false;
        }
    }
}
