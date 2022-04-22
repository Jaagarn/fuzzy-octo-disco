using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 10.0f;
    private Rigidbody playerRb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 movementVector;
    private int activeCamera;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        activeCamera = MasterCameraController.enabledCamera;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movementVector.y = 0;

        activeCamera = MasterCameraController.enabledCamera;

        InverseDependingOnCamera();

        if (Input.GetKeyDown(KeyCode.Space))
            playerRb.AddForce(Vector3.up * 200.0f);

        if (playerRb.position.y <= -1f)
            playerRb.position = new Vector3(0, 10f, 0);
    }
    
    private void InverseDependingOnCamera() 
    {
        switch (activeCamera)
        {
            case 1:
                movementVector.x = horizontalInput;
                movementVector.z = verticalInput;
                break;
            case 2:
                movementVector.x = verticalInput;
                movementVector.z = horizontalInput * -1;
                break;
            case 3:
                movementVector.x = horizontalInput * -1;
                movementVector.z = verticalInput * -1;
                break;
            case 4:
                movementVector.x = verticalInput * -1;
                movementVector.z = horizontalInput;
                break;
            default:
                movementVector.x = horizontalInput;
                movementVector.z = verticalInput;
                break;
        }
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(Vector3.forward * movementVector.z * speed);
        playerRb.AddForce(Vector3.right * movementVector.x * speed);
    }
}
