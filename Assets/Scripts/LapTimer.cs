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

    private Image backgroundImage;
    private Color backgroundImageNormal;
    private Color backgroundImageInvisible;

    private void Start()
    {
        lapTimerText = GameObject.FindGameObjectWithTag("LapTimerText");
        backgroundImage = lapTimerText.GetComponentInChildren<Image>();
        var tempColor = backgroundImage.color;
        backgroundImageNormal = tempColor;
        tempColor.a = 0f;
        backgroundImageInvisible = tempColor;
    }

    private void Update()
    {
        if(PlayerController.inMainHub)
            return;

        if(hasStartedLap)
            elapsedTime = Time.time - startTime;

        backgroundImage.color = backgroundImageNormal;

        lapTimerText.GetComponent<Text>().text = 
            $"Current lap time: " +
            $"{FormatTime(elapsedTime)}\n" +
            $"Checkpoint time: " +
            $"{checkPointTimeString}\n\n" +
            $"Best lap time: " +
            $"{bestTimeString}\n" +
            $"Best checkpoint time: " +
            $"{bestCheckPointTimeString}";
    }

    private void LateUpdate()
    {
        if(PlayerController.resetTimer)
        {
            elapsedTime = 0f;
            hasStartedLap = false;
            hasPassedCheckPoint = false;
            checkPointTimeString = "-";
            PlayerController.resetTimer = false;
        }

        if(PlayerController.inMainHub)
        {
            lapTimerText.GetComponent<Text>().text = string.Empty;
            backgroundImage.color = backgroundImageInvisible;
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
        if (other.CompareTag("FinishLine") && hasPassedCheckPoint && hasStartedLap)
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

        if (other.CompareTag("CheckPoint") && hasStartedLap && !hasPassedCheckPoint)
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

        if (other.CompareTag("StartLine") && !hasStartedLap && !hasPassedCheckPoint) 
        {
            hasStartedLap = true;
            checkPointTimeString = "-";
            startTime = Time.time;
        }

    }
}
