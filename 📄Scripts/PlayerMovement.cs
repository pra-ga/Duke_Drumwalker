using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float stepSize = 1f; // The distance to move per key press
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 1000f;
    [SerializeField] float jumpForce = 10f;
    Animator animator;
    Rigidbody rb;
    Vector3 targetPos;
    Vector3 drumPlus2Pos;
    Vector3 motionDirection;

    [Header("Drum")]
    [SerializeField] GameObject drum;
    [SerializeField] Transform drumSummonPoint;
    [SerializeField] Transform drumTopPos;
    bool isDrumSummoned = false;
    GameObject _drum = null;
    public float targetScaleY = 0.5f; // The desired Y scale
    public float duration = 1f; // How long the scaling animation takes
    Vector3 originalDrumScale;

    private bool isScaling = false;
    [SerializeField] GameObject door;
    Animator doorAnimator;

    [Header("Private")]
    int intNumberOfFruits = 0;
    int intNumberOfFruitsInScene = 0;
    int currentSceneInt;



    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
        doorAnimator = door.GetComponentInChildren<Animator>();

        currentSceneInt = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("currentSceneInt: " + currentSceneInt);
        LoadSceneVariables(currentSceneInt);
    }

    void LoadSceneVariables(int currentSceneInt)
    {
        switch (currentSceneInt)
        {
            case 0:
                break;
            case 1:
                intNumberOfFruitsInScene = 1;
                break; // Exits the switch statement
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed && (transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            Vector2 inputDirection = context.ReadValue<Vector2>();
            if (inputDirection.x > 0) // Move right
            {
                if (motionDirection != Vector3.right)
                {
                    motionDirection = Vector3.right;
                    Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    targetPos = transform.position + motionDirection * stepSize;
                    animator.SetTrigger("walk");
                }

            }
            else if (inputDirection.x < 0) // Move left
            {
                if (motionDirection != Vector3.left)
                {
                    motionDirection = Vector3.left;
                    Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    targetPos = transform.position + motionDirection * stepSize;
                    animator.SetTrigger("walk");
                }

            }
            else if (inputDirection.y > 0)
            {
                if (motionDirection != Vector3.forward)
                {
                    motionDirection = Vector3.forward;
                    Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    targetPos = transform.position + motionDirection * stepSize;
                    animator.SetTrigger("walk");


                }
            }
            else if (inputDirection.y < 0)
            {
                if (motionDirection != Vector3.back)
                {
                    motionDirection = Vector3.back;
                    Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    targetPos = transform.position + motionDirection * stepSize;
                    animator.SetTrigger("walk");
                }
            }
        }
    }

    public void OnDrum(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isDrumSummoned)
            {
                _drum = Instantiate(drum, drumSummonPoint.position, drumSummonPoint.rotation);
                //originalDrumScale = _drum.transform.localScale;
                //drumTopPos = _drum.transform.Find("DrumTopPos").transform;
                //Debug.Log("drumTopPos name: " + drumTopPos.name);
                isDrumSummoned = true;
            }
            else if (isDrumSummoned)
            {
                Destroy(_drum);
                isDrumSummoned = false;
            }
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        DoorOpenCheck();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drum")
        {
            transform.position = _drum.transform.position;
            //ScaleYSmoothly();
            drumPlus2Pos = other.transform.position + motionDirection.normalized * 2f;
            StartCoroutine(WaitOnTheDrum());
        }

        if (other.gameObject.tag == "Fruits")
        {
            intNumberOfFruits++;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Drum")
        {
            //other.gameObject.transform.localScale = originalDrumScale;
        }
    }

    IEnumerator WaitOnTheDrum()
    {
        yield return new WaitForSeconds(1f); // Wait for 3 seconds
        rb.useGravity = true;
        Vector3 Vec45 = transform.up.normalized + transform.forward.normalized;
        rb.AddForce(Vec45 * jumpForce, ForceMode.Impulse);
        targetPos = drumPlus2Pos;
    }

    void DoorOpenCheck()
    {
        if (intNumberOfFruits == intNumberOfFruitsInScene)
        {
            doorAnimator.SetTrigger("open");
        }
    }

    void ScaleYSmoothly()
    {
        if (!isScaling)
        {
            StartCoroutine(ScaleOverTime(_drum.transform, new Vector3(_drum.transform.localScale.x, targetScaleY, transform.localScale.z), duration));
        }
    }

    private IEnumerator ScaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
        isScaling = true;
        float counter = 0;
        Vector3 startScale = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScale, toScale, counter / duration);
            yield return null; // Wait for the next frame
        }

        objectToScale.localScale = toScale; // Ensure the target scale is reached precisely
        isScaling = false;
    }
    
}