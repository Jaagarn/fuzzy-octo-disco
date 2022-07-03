using UnityEngine;

public class CrazyRotate : MonoBehaviour
{
    private const float rotationSpeed = 20.0f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime));
    }
}
