using System.Collections;
using UnityEngine;

public class ForksUpAnimation : MonoBehaviour
{
    public bool forksActive = true;
    public Animator animator;
    [SerializeField] float trapDelay = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(SetTriggerAfterDelay(trapDelay));
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator SetTriggerAfterDelay(float delay)
    {
        while (forksActive)
        {
            yield return new WaitForSeconds(delay);
            animator.SetTrigger("ForksUp");
        }
        
    }
}
