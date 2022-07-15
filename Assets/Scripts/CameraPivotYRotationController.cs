using UnityEngine;

public class CameraPivotYRotationController : MonoBehaviour
{
    private GameObject player;
    private Quaternion originalAngle;
    private const int roatationSpeed = 120;

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

        var rotation = horizontalInput * roatationSpeed * Time.deltaTime;

        transform.RotateAround(player.transform.position, Vector3.up, rotation);
    }

    private void LateUpdate()
    {
        var currentPosition = transform.position;
        var nextPositon = player.transform.position;

        transform.position = Vector3.Lerp(currentPosition, nextPositon, 1);
    }

    private void ResetEventHandler()
    {
        transform.SetPositionAndRotation(transform.position, originalAngle);
    }
}