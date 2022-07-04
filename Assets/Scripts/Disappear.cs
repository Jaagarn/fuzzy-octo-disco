using UnityEngine;

public class Disappear : MonoBehaviour
{
    private const float visibleTime = 3.0f;
    private float nextVisibleTime = 0.0f;
    private bool isVisible = false;
    private int ticks = 0;

    private Color originalColor;
    private Color transparentColor;

    private void Start()
    {
        originalColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor = gameObject.GetComponent<Renderer>().material.color;
        transparentColor.a = 0f;
    }

    private void Update()
    {
        if(Time.time > nextVisibleTime)
        {
            nextVisibleTime = Time.time + visibleTime;
            ticks++;

            if (!isVisible)
            {
                GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<Renderer>().material.color = originalColor;
                isVisible = true;
                ticks = 0;
            }
            else if (ticks == 4)
            {
                GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Renderer>().material.color = transparentColor;
                isVisible = false;
            }
        }
    }
}
