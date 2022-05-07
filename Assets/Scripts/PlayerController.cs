using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private float maxSpeed = 160.0f;
    private Rigidbody playerRb;
    private GameObject mainCamera;
    private float verticalInput;
    private bool isBreaking = false;
    private bool isGrounded = true;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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

        if (playerRb.position.y <= -1f)
            playerRb.position = new Vector3(0, 0.5f, 0);
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
