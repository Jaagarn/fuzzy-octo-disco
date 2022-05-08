using UnityEngine;

public class Disappear : MonoBehaviour
{
    private float visibleTime = 5.0f;
    private float nextVisibleTime = 0.0f;
    bool isVisible = false;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextVisibleTime)
        {
            nextVisibleTime = Time.time + visibleTime;

            if (!isVisible)
            {
                GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                isVisible = true;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                isVisible = false;
            }
        }

    }

}
