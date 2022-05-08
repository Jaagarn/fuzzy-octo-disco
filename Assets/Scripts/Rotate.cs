using UnityEngine;

public class Rotate : MonoBehaviour
{
    float rotationSpeed = 20.0f;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
