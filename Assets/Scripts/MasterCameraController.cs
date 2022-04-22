using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera firstCamera;
    [SerializeField]
    private Camera secondCamera;
    [SerializeField]
    private Camera thirdCamera;
    [SerializeField]
    private Camera fourthCamera;

    public static int enabledCamera;
    public void Start()
    {
        DiableAllCameras();
        firstCamera.enabled = true;
        enabledCamera = 1;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (enabledCamera == 1)
                enabledCamera = 4;
            else
                enabledCamera -= 1;

            UpdateSelectedCamera();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (enabledCamera == 4)
                enabledCamera = 1;
            else
                enabledCamera += 1;

            UpdateSelectedCamera();
        }
    }

    private void UpdateSelectedCamera()
    {
        DiableAllCameras();

        switch (enabledCamera)
        {
            case 1:
                firstCamera.enabled = true;
                break;
            case 2:
                secondCamera.enabled = true;
                break;
            case 3:
                thirdCamera.enabled = true;
                break;
            case 4:
                fourthCamera.enabled = true;
                break;

        }
    }

    private void DiableAllCameras() 
    {
        firstCamera.enabled = false;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        fourthCamera.enabled = false;
    }
}
