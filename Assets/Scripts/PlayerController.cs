using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private float maxSpeed = 160.0f;
    private Rigidbody playerRb;
    private GameObject mainCamera;
    private Vector3 startingPlayerPosition;
    private float verticalInput;
    private bool isBreaking = false;
    private bool isGrounded = true;

    // Make event. Lazy
    public static bool resetCamera = false;

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
            playerRb.AddForce(Vector3.up * 200.0f);

        if (playerRb.position.y <= -2.0f)
        {
            playerRb.position = startingPlayerPosition;
            resetCamera = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        if(!isBreaking && isGrounded && !(playerRb.velocity.magnitude >= maxSpeed))
        {
            var verticallVector = (mainCamera.transform.forward * verticalInput * speed);
            verticallVector.y = 0;

            playerRb.AddForce(verticallVector);
        }
    }
}
