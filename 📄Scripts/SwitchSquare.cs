using UnityEngine;

public class SwitchSquare : MonoBehaviour
{
    [SerializeField] GameObject fireTrap;
    Animator switchAnimator;
    FireTileManager fireTileManager;

    void Start()
    {
        switchAnimator = GetComponentInChildren<Animator>();
        fireTileManager = fireTrap.GetComponent<FireTileManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Drum")
        {
            switchAnimator.SetTrigger("toggle-on");
            fireTileManager.fireParticle.Stop();
        }
    }
}
