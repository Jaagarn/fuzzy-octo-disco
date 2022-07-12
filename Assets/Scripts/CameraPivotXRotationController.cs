using UnityEngine;

public class CameraPivotXRotationController : MonoBehaviour
{
    private float xAngle;
    private bool positveOrNegativeAngle;
    private bool hasVerticalInput;
    private const int roatationSpeed = 60;

    void Start()
    {
        xAngle = transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            hasVerticalInput = true;
            positveOrNegativeAngle = true;
        }


        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            hasVerticalInput = true;
            positveOrNegativeAngle = false;
        }

        if (hasVerticalInput)
        {
            RotateAroundX();
            hasVerticalInput = false;
        }
    }

    private void RotateAroundX()
    {
        if (positveOrNegativeAngle)
        {
            if (xAngle >= 75.0f)
                return;

            xAngle += Time.deltaTime * roatationSpeed;
        }
        else
        {
            if (xAngle <= 0.0f)
                return;

            xAngle -= Time.deltaTime * roatationSpeed;
        }

        var currentAngels = transform.rotation.eulerAngles;
        currentAngels.x = xAngle;

        transform.eulerAngles = currentAngels;
    }
}
