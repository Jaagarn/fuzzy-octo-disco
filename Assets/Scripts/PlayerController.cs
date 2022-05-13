using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float speed = 10.0f;
    private const float maxSpeed = 160.0f;
    private Rigidbody playerRb;
    private GameObject mainCamera;
    private Vector3 startingPlayerPosition;
    private float verticalInput;
    private bool isBreaking = false;
    private bool isGrounded = true;
    private float speedModifier = 1.0f;

    // Make event. Lazy
    // This is truly terrible
    public static bool resetCamera = false;
    public static bool resetTimer = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        startingPlayerPosition = playerRb.position;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            playerRb.angularDrag = 15.0f;
            isBreaking = true;
        }
            
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerRb.angularDrag = 0.05f;
            isBreaking = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * 200.0f);
            isGrounded = false;
        }

        if (playerRb.position.y <= -4.0f || Input.GetKeyDown(KeyCode.R))
        {
            playerRb.position = startingPlayerPosition;
            playerRb.velocity = Vector3.zero;
            resetCamera = true;
            resetTimer = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FourthCamera"))
        {
            playerRb.position = new Vector3(28f, 5f, 2.6f);
            playerRb.velocity = Vector3.zero;
            resetCamera = true;
            resetTimer = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Fetch parent of colliding gameObject to fetch data from parentSlideController
        GameObject collidingObjectParent = collision.gameObject.transform.parent.gameObject;
        if(collidingObjectParent.tag.Equals("Slide"))
            speedModifier = collidingObjectParent.GetComponent<ParentSlideController>().speedModifier;
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        speedModifier = 1.0f;
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        if(!isBreaking && isGrounded && !(playerRb.velocity.magnitude >= maxSpeed))
        {
            // Create new vector3 from camera vector3 without rotation
            Vector3 moveDirectionVector = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
            Vector3 verticallVector = moveDirectionVector * verticalInput * speed * speedModifier;
            playerRb.AddForce(verticallVector);
        }
    }
}
