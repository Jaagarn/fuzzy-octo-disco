using UnityEngine;

public class ParentSlideController : MonoBehaviour
{
    [SerializeField]
    private bool isDisappearing = false;
    [SerializeField]
    private float visibleTimeSeconds = 10.0f;
    [SerializeField]
    private float invisibleTimeSeconds = 4.0f;
    [SerializeField]
    private bool startAsInvisible = false;
    private float timer = 0.0f;
    private bool isVisible = false;
    private float timeToWait;

    // Broadcast to all children to become visible
    private void Start()
    {
        if(!isDisappearing)
        {
            return;
        }

        transform.BroadcastMessage(startAsInvisible ? "BecomeInvisible" : "BecomeVisible");
        timeToWait = startAsInvisible ? invisibleTimeSeconds : visibleTimeSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDisappearing)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer > timeToWait)
        {
           if(isVisible)
           {
                transform.BroadcastMessage("BecomeInvisible");
                isVisible = false;
                timer -= timeToWait;
                timeToWait = invisibleTimeSeconds;
           }
           else
           {
                transform.BroadcastMessage("BecomeVisible");
                isVisible = true;
                timer -= timeToWait;
                timeToWait = visibleTimeSeconds;
           }
        }
    }

    // Parent needs to implement methods called by broadcast
    private void BecomeVisible(){}

    private void BecomeInvisible(){}
}
