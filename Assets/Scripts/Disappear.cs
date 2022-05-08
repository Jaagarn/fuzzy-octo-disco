using UnityEngine;

public class Disappear : MonoBehaviour
{
    private float visibleTime = 5.0f;
    private float nextVisibleTime = 0.0f;
    private bool isVisible = false;

    private Color originalColor;
    private Color transparentColor;

    private void Start()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextVisibleTime)
        {
            nextVisibleTime = Time.time + visibleTime;

            if (!isVisible)
            {
                GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<Renderer>().material.color = originalColor;
                isVisible = true;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Renderer>().material.color. = transparentColor;
                isVisible = false;
            }
        }

    }

}
