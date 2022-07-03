using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FirstTrack is also called the green one
/// ThridTrack is also called the yellow one
/// </summary>
public class LapTimer : MonoBehaviour
{
    PlayerTeleportLocation playerCurrentLocation = PlayerTeleportLocation.MainHub;

    private bool hasStartedLap = false;
    private bool inMainHub = true;
    private bool hasPassedCheckPoint = false;

    private float startTime;
    private float elapsedTime;
    private float checkPointTime;

    private float bestFirstTrackTime;
    private float bestFirstTrackCheckPointTime;

    private float bestThirdTrackTime;
    private float bestThirdTrackCheckPointTime;

    private GameObject lapTimerText;
    private GameObject lapTimerCheckPointText;
    private GameObject lapTimerTextWrapper;

    private GameObject bestTimeWrapper;

    private GameObject greenTimeWrapper;
    private GameObject bestCheckPointTimeGreenText;
    private GameObject bestLapTimeGreenText;

    private GameObject yellowTimeWrapper;
    private GameObject bestCheckPointTimeYellowText;
    private GameObject bestLapTimeYellowText;


    private void OnEnable()
    {
        StateAndLocatizationEventManager.OnReset += ResetEventHandler;
        StateAndLocatizationEventManager.OnLocationChange += LocationChangedEventHandler;
    }

    private void OnDisable()
    {
        StateAndLocatizationEventManager.OnReset -= ResetEventHandler;
        StateAndLocatizationEventManager.OnLocationChange -= LocationChangedEventHandler;
    }

    private void Start()
    {
        lapTimerText = GameObject.FindGameObjectWithTag("LapTimerText");
        lapTimerCheckPointText = GameObject.FindGameObjectWithTag("LapTimerCheckPointText");
        lapTimerTextWrapper = GameObject.FindGameObjectWithTag("LapTimerWrapper");

        bestCheckPointTimeGreenText = GameObject.FindGameObjectWithTag("BestCheckPointTimeGreenText");
        bestLapTimeGreenText = GameObject.FindGameObjectWithTag("BestLapTimeGreenText");
        greenTimeWrapper = GameObject.FindGameObjectWithTag("BestGreenWrapper");

        bestCheckPointTimeYellowText = GameObject.FindGameObjectWithTag("BestCheckPointTimeYellowText");
        bestLapTimeYellowText = GameObject.FindGameObjectWithTag("BestLapTimeYellowText");
        yellowTimeWrapper = GameObject.FindGameObjectWithTag("BestYellowWrapper");

        bestTimeWrapper = GameObject.FindGameObjectWithTag("BestTimeWrapper");

        greenTimeWrapper.SetActive(false);
        yellowTimeWrapper.SetActive(false);
        MainHubUI();
    }

    private void Update()
    {
        if (inMainHub)
            return;

        if (hasStartedLap)
            elapsedTime = Time.time - startTime;

        var formattedTime = FormatTime(elapsedTime);

        lapTimerText.GetComponent<Text>().text = formattedTime;
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)((time * 100.0f) % 100.0f);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine") && hasPassedCheckPoint && hasStartedLap)
        {
            var lapTime = elapsedTime;
            hasStartedLap = false;
            hasPassedCheckPoint = false;

            UpdateLapTimeIfImproved(lapTime);
        }

        if (other.CompareTag("CheckPoint") && hasStartedLap && !hasPassedCheckPoint)
        {
            hasPassedCheckPoint = true;
            checkPointTime = elapsedTime;
            lapTimerCheckPointText.GetComponent<Text>().text = FormatTime(checkPointTime);

            UpdateCheckPointTimeIfImproved();
        }

        if (other.CompareTag("StartLine") && !hasStartedLap && !hasPassedCheckPoint)
        {
            hasStartedLap = true;

            lapTimerCheckPointText.GetComponent<Text>().text = string.Empty;
            startTime = Time.time;
        }

    }

    private void ResetEventHandler()
    {
        elapsedTime = 0f;
        checkPointTime = 0f;
        hasStartedLap = false;
        hasPassedCheckPoint = false;
        lapTimerCheckPointText.GetComponent<Text>().text = string.Empty;
    }

    private void MainHubUI()
    {
        lapTimerTextWrapper.SetActive(false);
        bestTimeWrapper.SetActive(true);
    }

    private void TrackUI()
    {
        lapTimerTextWrapper.SetActive(true);
        bestTimeWrapper.SetActive(false);
    }

    private void UpdateCheckPointTimeIfImproved() 
    {
        switch (playerCurrentLocation)
        {
            case PlayerTeleportLocation.FirstTrack:
                if (bestFirstTrackCheckPointTime == default || 
                    checkPointTime < bestFirstTrackCheckPointTime)
                {
                    bestFirstTrackCheckPointTime = checkPointTime;
                    bestCheckPointTimeGreenText.GetComponent<Text>().text = FormatTime(bestFirstTrackCheckPointTime);
                }
                break;
            case PlayerTeleportLocation.ThirdTrack:
                if (bestThirdTrackCheckPointTime == default ||
                    checkPointTime < bestThirdTrackCheckPointTime)
                {
                    bestThirdTrackCheckPointTime = checkPointTime;
                    bestCheckPointTimeYellowText.GetComponent<Text>().text = FormatTime(bestThirdTrackCheckPointTime);
                }
                break;
            default:
                break;
        }
    }

    private void UpdateLapTimeIfImproved(float lapTime)
    {
        switch (playerCurrentLocation)
        {
            case PlayerTeleportLocation.FirstTrack:
                if (bestFirstTrackTime == default ||
                    lapTime < bestFirstTrackTime)
                {
                    bestFirstTrackTime = lapTime;
                    bestLapTimeGreenText.GetComponent<Text>().text = FormatTime(bestFirstTrackTime);
                }
                if (!greenTimeWrapper.activeSelf)
                    greenTimeWrapper.SetActive(true);
                break;
            case PlayerTeleportLocation.ThirdTrack:
                if (bestThirdTrackTime == default ||
                    lapTime < bestThirdTrackTime)
                {
                    bestThirdTrackTime = lapTime;
                    bestLapTimeYellowText.GetComponent<Text>().text = FormatTime(bestThirdTrackTime);
                }
                if (!yellowTimeWrapper.activeSelf)
                    yellowTimeWrapper.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void LocationChangedEventHandler(PlayerTeleportLocation teleportLocation)
    {
        playerCurrentLocation = teleportLocation;

        switch(teleportLocation)
        {
            case PlayerTeleportLocation.MainHub:
                inMainHub = true;
                MainHubUI();
                break;
            default:
                inMainHub = false;
                TrackUI();
                break;
        }
            
    }
}
