using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject player;
    private Quaternion originalAngle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalAngle = transform.rotation;
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        var rotation = horizontalInput * -90 * Time.deltaTime;

        transform.RotateAround(player.transform.position, Vector3.down, rotation);

        if (Input.GetKey(KeyCode.Alpha1))
            transform.SetPositionAndRotation(transform.position, originalAngle);

    }

    void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}