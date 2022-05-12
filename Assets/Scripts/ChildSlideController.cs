using UnityEngine;

public class ChildSlideController : MonoBehaviour
{
    private Color originalColor;
    private Color transparentColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor = originalColor;
        transparentColor.a = 0f;
    }

    // Called by broadcasting from parent
    private void BecomeVisible()
    {
        GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Renderer>().material.color = originalColor;
    }

    // Called by broadcasting from parent
    private void BecomeInvisible()
    {
        GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Renderer>().material.color = transparentColor;
    }
}
