using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float rotationSpeed = 20.0f;

    [SerializeField]
    private bool reverseRotate = false;

    private void Start()
    {
        if (reverseRotate)
            rotationSpeed *= -1.0f;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
