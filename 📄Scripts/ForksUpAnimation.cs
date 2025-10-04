using System.Collections;
using UnityEngine;

public class ForksUpAnimation : MonoBehaviour
{
    public bool forksActive = true;
    public Animator animator;
    [SerializeField] float trapDelay = 3f;
    bool isForkUp = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(SetTriggerAfterDelay(trapDelay));
    }

    IEnumerator SetTriggerAfterDelay(float delay)
    {
        while (forksActive)
        {
            yield return new WaitForSeconds(delay);
            isForkUp = !isForkUp; //toggle
            animator.SetBool("isForkUp", isForkUp);
        }
        
        isForkUp = false;
        animator.SetBool("isForkUp", isForkUp);
    }
}
