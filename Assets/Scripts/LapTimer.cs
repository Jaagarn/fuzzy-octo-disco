using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour
{
    private bool hasStartedLap = false;

    private float startTime;
    private float elapsedTime;

    private float bestTime;

    private float checkPointTime;
    private float bestCheckPointTime;

    private string checkPointTimeString = "-";
    private string bestTimeString = "-";
    private string bestCheckPointTimeString = "-";
    
    private bool hasPassedCheckPoint = false;
    private GameObject lapTimerText;

    private void Start()
    {
        lapTimerText = GameObject.FindGameObjectWithTag("LapTimerText");
    }

    private void FixedUpdate()
    {
        if (hasStartedLap)
        {
            elapsedTime = Time.time - startTime;
        }

        lapTimerText.GetComponent<Text>().text = $"Current lap time: {FormatTime(elapsedTime)}\nCheckpoint time: {checkPointTimeString}\n\nBest lap time: {bestTimeString}\nBest checkpoint time: {bestCheckPointTimeString}";
    }

    private void LateUpdate()
    {
        if(PlayerController.resetTimer)
        {
            elapsedTime = 0f;
            hasStartedLap = false;
            hasPassedCheckPoint = false;
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
        if (other.CompareTag("FinishLine") && hasPassedCheckPoint)
        {
            var lapTime = elapsedTime;
            hasStartedLap = false;
            hasPassedCheckPoint = false;

            if (bestTime == default)
            {
                bestTime = lapTime;
                bestTimeString = FormatTime(bestTime);
            }
            else if (bestTime > lapTime)
            {
                bestTime = lapTime;
                bestTimeString = FormatTime(bestTime);
            }
        }

        if (other.CompareTag("CheckPoint"))
        {
            hasPassedCheckPoint = true;
            checkPointTime = elapsedTime;
            checkPointTimeString = FormatTime(checkPointTime);

            if (bestCheckPointTime == default)
            {
                bestCheckPointTime = checkPointTime;
                bestCheckPointTimeString = FormatTime(bestCheckPointTime);
            }
            else if (bestCheckPointTime > checkPointTime)
            {
                bestCheckPointTime = checkPointTime;
                bestCheckPointTimeString = FormatTime(bestCheckPointTime);
            }
        }

        if (other.CompareTag("StartLine")) 
        {
            hasStartedLap = true;
            checkPointTimeString = "-";
            startTime = Time.time;
        }

    }
}
