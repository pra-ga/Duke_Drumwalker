using UnityEngine;

public class ForwardChecker : MonoBehaviour
{
    public string forwardTag;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Other tag: " + other.tag);
        forwardTag = other.tag;
    }

    void OnTriggerExit(Collider other)
    {
        forwardTag = null;
    }
}
