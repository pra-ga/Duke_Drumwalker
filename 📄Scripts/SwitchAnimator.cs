using Unity.VisualScripting;
using UnityEngine;

public class SwitchAnimator : MonoBehaviour
{
    [SerializeField] GameObject forksTrap;
    Animator switchAnimator;
    Animator forksTrapAnimator;
    ForksUpAnimation forksUpAnimation;
    void Start()
    {
        switchAnimator = GetComponentInChildren<Animator>();
        forksTrapAnimator = GetComponentInChildren<Animator>();
        forksUpAnimation = forksTrap.GetComponentInChildren<ForksUpAnimation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Drum")
        {
            switchAnimator.SetTrigger("toggle-on");
            forksUpAnimation.animator.SetTrigger("ForksDown");
            forksUpAnimation.forksActive = false;
        }
    }
}
