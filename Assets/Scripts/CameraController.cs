using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Quaternion originalAngle;
    private const int invertRoatationSpeed = -90;

    private void OnEnable()
    {
        StateAndLocatizationEventManager.OnReset += ResetEventHandler;
    }

    private void OnDisable()
    {
        StateAndLocatizationEventManager.OnReset -= ResetEventHandler;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalAngle = transform.rotation;
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        var rotation = horizontalInput * invertRoatationSpeed * Time.deltaTime;

        transform.RotateAround(player.transform.position, Vector3.down, rotation);

    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }

    private void ResetEventHandler()
    {
        transform.SetPositionAndRotation(transform.position, originalAngle);
    }

}