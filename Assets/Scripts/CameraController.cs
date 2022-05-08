using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject player;
    private Quaternion originalAngle;
    private const int invertRoatationSpeed = -90;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalAngle = transform.rotation;
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        var rotation = horizontalInput * invertRoatationSpeed * Time.deltaTime;

        transform.RotateAround(player.transform.position, Vector3.down, rotation);

        if (Input.GetKey(KeyCode.Alpha1))
            transform.SetPositionAndRotation(transform.position, originalAngle);
    }

    void LateUpdate()
    {
        transform.position = player.transform.position;

        // Make to an event. You lazy
        if (PlayerController.resetCamera)
        {
            transform.SetPositionAndRotation(transform.position, originalAngle);
            PlayerController.resetCamera = false;
        }
    }
}