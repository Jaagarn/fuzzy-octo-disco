using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour
{
    private bool hasStartedLap = false;
    private float elapsedTime;
    private float bestTime;
    private string bestTimeString = "-";
    private float startTime;
    private GameObject lapTimerText;

    private void Start()
    {
        lapTimerText = GameObject.FindGameObjectWithTag("LapTimerText");
    }

    private void FixedUpdate()
    {
        if (hasStartedLap)
            elapsedTime = Time.time - startTime;

        lapTimerText.GetComponent<Text>().text = $"Current lap time: {FormatTime(elapsedTime)} \nBest time: {bestTimeString}";
    }

    private void LateUpdate()
    {
        if(PlayerController.resetTimer)
        {
            elapsedTime = 0f;
            hasStartedLap = false;
            PlayerController.resetTimer = false;
        }
    }

    public string FormatTime(float time)
    {
        int minutes = (int)time / 60; 
        int seconds = (int)time % 60;
        int milliseconds =(int) ((time * 100.0f) % 100.0f);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartLine")) 
        {
            hasStartedLap = true;
            startTime = Time.time;
        }

        if (other.CompareTag("FinishLine"))
        {
            hasStartedLap = false;
            if (bestTime == default)
            {
                bestTime = elapsedTime;
                bestTimeString = FormatTime(bestTime);
            }
            else if (bestTime > elapsedTime) 
            {
                bestTime = elapsedTime;
                bestTimeString = FormatTime(bestTime);
            }
                
        }
    }
}
