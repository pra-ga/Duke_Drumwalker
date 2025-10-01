using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#region Todo
// BUG : ✅Prevent jump if there is no floor across jelly. Check by ray casting 3 tiles away.
// BUG : ✅Jelly should get summoned only on a tile
// BUG : ✅Remove dependecy on JumpForce. Instead check the cooridinate of the next tile.
// BUG : When the player refuses to jump on jelly, he does not turn
// TODO: Summon particle effects
// TODO: Fix raycast
// TODO: States enum
// TODO: Audio
// TODO: Particle effects   
// TODO: UI
// TODO: Menu   
#endregion

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
    enum States { Idle, Walk, Jump, Summon, DoorOpen, EmoteNo };
    States State;

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
    Animator jellyAnimator;

    [Header("Private")]
    int intNumberOfFruits = 0;
    int intNumberOfFruitsInScene = 0;
    int currentSceneInt;

    [Header("RayCast Checker")]
    [SerializeField] Transform raycastOrigin;
    [SerializeField] Transform jellyRaycastOrigin;
    [SerializeField] float raycastDistanceToCheckWalk = 1f; // Set the raycast distance to check next tile
    [SerializeField] float raycastDistanceToCheckJump = 3f; // Set the raycast distance to check next tile
    Vector3 playerToJellyDirection = Vector3.zero;
    JumpChecker jumpChecker;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
        doorAnimator = door.GetComponentInChildren<Animator>();

        currentSceneInt = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("currentSceneInt: " + currentSceneInt);
        LoadSceneVariables(currentSceneInt);
        //forwardChecker = GetComponentInChildren<ForwardChecker>();
        State = States.Idle; //Initially idle
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
        if (context.performed && State == States.Idle)
        {
            if (IsJellyAhead())
            {
                Debug.Log("IsJellyAhead");
                
                {
                    Debug.Log("CanPlayerJump");
                    {
                        //State = States.Walk;
                        Vector2 inputDirection = context.ReadValue<Vector2>();
                        if (inputDirection.x > 0) // Move right
                        {
                            if (motionDirection != Vector3.right) //rotation
                            {
                                motionDirection = Vector3.right;
                                Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                if (canWalkForward() && jumpChecker.CanPlayerJump())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward() && jumpChecker.CanPlayerJump())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward() && jumpChecker.CanPlayerJump())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward() && jumpChecker.CanPlayerJump())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
                            }
                        }
                    }
                }
                /* else
                {
                    EmoteNoAnimation();
                    State = States.Idle;
                }*/
                State = States.Idle; 
            }
            else if (!IsJellyAhead())
            {
                Debug.Log("Not IsJellyAhead");
                
                {
                    Debug.Log("canWalk");
                    {
                        //State = States.Walk;
                        Vector2 inputDirection = context.ReadValue<Vector2>();
                        if (inputDirection.x > 0) // Move right
                        {
                            if (motionDirection != Vector3.right) //rotation
                            {
                                motionDirection = Vector3.right;
                                Quaternion targetRotation = Quaternion.LookRotation(motionDirection, Vector3.up);
                                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                            }
                            else
                            {
                                if (canWalkForward())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
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
                                if (canWalkForward())
                                {
                                    targetPos = transform.position + motionDirection * stepSize;
                                    animator.SetTrigger("walk");
                                }
                                else EmoteNoAnimation();
                            }
                        }

                        State = States.Idle;
                    }
                }
                /* else
                {
                    Debug.Log("CannotWalk");
                    EmoteNoAnimation();
                    State = States.Idle;
                } */
            }
        }
        /* if (IsJellyAhead())
        {
            Debug.Log("IsJellyAhead");
            if (jumpChecker.CanPlayerJump())
            {
                Debug.Log("CanPlayerJump");
                MovePlayer(context);
                State = States.Idle;

            }
            else
            {
                Debug.Log("CannotJump");
                if (canWalk())
                {
                    MovePlayer(context);
                    State = States.Idle;    
                }
            }
        }
        else if (!IsJellyAhead())
        {
            MovePlayer(context);
            State = States.Idle;
        } */
    }

/*     private void MovePlayer(InputAction.CallbackContext context)
    {

        if (context.performed 
        // && (transform.position - targetPos).sqrMagnitude < 0.01f  
        && State == States.Idle)
        {

            //State = States.Walk;
            Vector2 inputDirection = context.ReadValue<Vector2>();
            if (inputDirection.x > 0) // Move right
            {
                if (motionDirection != Vector3.right) //rotation
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
        //Set to idle if no input or too far from the target position
        else State = States.Idle;
    } 
    */

    /* private void MoveOnlyIfThereIsATile()
    {
        Debug.Log("MoveOnlyIfThereIsATile");
        if (canWalk())
        {
            targetPos = transform.position + motionDirection * stepSize;
            animator.SetTrigger("walk");
        }
        else
        {
            EmoteNoAnimation();
        }
    } */

    private void EmoteNoAnimation()
    {
        animator.SetTrigger("No");
    }

    public void OnDrum(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canWalkForward())
            {
                if (!isDrumSummoned && State == States.Idle)
                {
                    animator.SetTrigger("Summon");
                    _drum = Instantiate(drum, drumSummonPoint.position, drumSummonPoint.rotation);
                    jellyAnimator = _drum.GetComponentInChildren<Animator>();
                    jumpChecker = _drum.GetComponentInChildren<JumpChecker>();
                    jellyAnimator.Play("Jelly-Place");
                    //originalDrumScale = _drum.transform.localScale;
                    //drumTopPos = _drum.transform.Find("DrumTopPos").transform;
                    isDrumSummoned = true;
                    playerToJellyDirection = _drum.transform.position - transform.position;
                }
                else if (isDrumSummoned)
                {
                    Destroy(_drum);
                    isDrumSummoned = false;
                }
            }
            else
            {
                EmoteNoAnimation();
            }
        }
    }

    void Update()
    {
        Debug.Log("State: " + State);
        if ((transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            State = States.Idle;
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),
                                             Mathf.RoundToInt(transform.position.y),
                                             Mathf.RoundToInt(transform.position.z));
        }
        else State = States.Walk;

        if (State == States.Walk)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        DoorOpenCheck();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drum")
        {
            State = States.Jump;
            transform.position = new Vector3(_drum.transform.position.x, _drum.transform.position.y, _drum.transform.position.z); //_drum.transform.position;
            jellyAnimator = _drum.GetComponentInChildren<Animator>();
            jellyAnimator.SetTrigger("wobble");
            //ScaleYSmoothly();
            drumPlus2Pos = other.transform.position + motionDirection.normalized * 2f;
            StartCoroutine(JellyJump());
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
            State = States.Idle;
        }
    }

    IEnumerator JellyJump()
    {
        yield return new WaitForSeconds(1f);
        rb.useGravity = true;
        Vector3 Vec45 = transform.up.normalized + transform.forward.normalized;
        rb.AddForce(Vec45 * jumpForce, ForceMode.Impulse);
        targetPos = drumPlus2Pos;
        yield return new WaitForSeconds(1.0f);
        rb.useGravity = false;
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

    bool IsJellyAhead()
    {
        Vector3 origin = jellyRaycastOrigin.position;
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, raycastDistanceToCheckWalk))
        {
            Debug.DrawRay(origin, direction * raycastDistanceToCheckWalk, Color.hotPink);
            if (hit.transform.CompareTag("Drum"))
            {
                return true;
            }

        }
        return false;


    }

    bool canWalkForward()
    {
        Vector3 origin = raycastOrigin.position;
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, raycastDistanceToCheckWalk))
        {
            if (hit.transform.tag == "Tile" || hit.transform.tag == "Fruits" || hit.transform.tag == "ExitDoor")
            {
                return true;
            }
        }
        return false;
    }

    string RayCastToCheckJump()
    {
        Vector3 origin = raycastOrigin.position;
        RaycastHit hit;

        if (Physics.Raycast(origin, playerToJellyDirection, out hit, raycastDistanceToCheckJump))
        {
            Debug.DrawLine(origin, origin + raycastDistanceToCheckJump * playerToJellyDirection, Color.blue);
            return hit.transform.tag;
        }
        else
        {
            return "none";
        }
    }

}