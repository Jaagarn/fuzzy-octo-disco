using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float rotateSpeed = 30.0f;
    private float speed = 5.0f;
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(Vector3.forward * speed * verticalInput * -1);
        playerRb.AddForce(Vector3.right * speed * horizontalInput * -1);

        if (horizontalInput > 0)
            transform.Rotate(Vector3.forward * rotateSpeed * verticalInput);

        if (verticalInput > 0)
            transform.Rotate(Vector3.right * rotateSpeed * verticalInput);

        if (Input.GetKeyDown( KeyCode.Space))
            playerRb.AddForce(Vector3.up * 100.0f);

        if (playerRb.position.y <= -1f)
            playerRb.position = new Vector3(0, 10f, 0);
    }
}
