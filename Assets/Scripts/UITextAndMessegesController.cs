using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FirstTrack is also called the green one
/// ThridTrack is also called the yellow one
/// </summary>
public class UITextAndMessegesController : MonoBehaviour
{
    [SerializeField]
    private Animator messageTextAnimator;

    PlayerTeleportLocation playerCurrentLocation = PlayerTeleportLocation.MainHub;

    private bool hasStartedLap = false;
    private bool inMainHub = true;
    private bool hasPassedCheckPoint = false;
    private bool allTracksClearedEventSent = false;
    private bool displayBestTrackTimes = false;

    private float startTime;
    private float elapsedTime;
    private float checkPointTime;

    private float bestFirstTrackTime;
    private float bestFirstTrackCheckPointTime;

    private float bestSecondTrackTime;
    private float bestSecondTrackCheckPointTime;

    private float bestThirdTrackTime;
    private float bestThirdTrackCheckPointTime;

    private GameObject messageText;

    private GameObject lapTimerText;
    private GameObject lapTimerCheckPointText;
    private GameObject lapTimerTextWrapper;

    private GameObject bestTimeInCurrentTrackLapTimeText;
    private GameObject bestTimeInCurrentTrackCheckPointText;
    private GameObject bestTimeInCurrentTrackWrapper;

    private GameObject bestTimeWrapper;

    private GameObject greenTimeWrapper;
    private GameObject bestCheckPointTimeGreenText;
    private GameObject bestLapTimeGreenText;

    private GameObject redTimeWrapper;
    private GameObject bestCheckPointTimeRedText;
    private GameObject bestLapTimeRedText;

    private GameObject yellowTimeWrapper;
    private GameObject bestCheckPointTimeYellowText;
    private GameObject bestLapTimeYellowText;

    private const string startMessage =
        "Rollers the game\n" +
        "by\n" +
        "Jaagarn\n" +
        "with help from\n" +
        "Keeso";


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

        bestTimeInCurrentTrackLapTimeText = GameObject.FindGameObjectWithTag("BestTimeInCurrentTrackLapTimeText");
        bestTimeInCurrentTrackCheckPointText = GameObject.FindGameObjectWithTag("BestTimeInCurrentTrackCheckPointTimeText");
        bestTimeInCurrentTrackWrapper = GameObject.FindGameObjectWithTag("BestTimeInCurrentTrackWrapper");

        bestCheckPointTimeGreenText = GameObject.FindGameObjectWithTag("BestCheckPointTimeGreenText");
        bestLapTimeGreenText = GameObject.FindGameObjectWithTag("BestLapTimeGreenText");
        greenTimeWrapper = GameObject.FindGameObjectWithTag("BestGreenWrapper");

        bestCheckPointTimeRedText = GameObject.FindGameObjectWithTag("BestCheckPointTimeRedText");
        bestLapTimeRedText = GameObject.FindGameObjectWithTag("BestLapTimeRedText");
        redTimeWrapper = GameObject.FindGameObjectWithTag("BestRedWrapper");

        bestCheckPointTimeYellowText = GameObject.FindGameObjectWithTag("BestCheckPointTimeYellowText");
        bestLapTimeYellowText = GameObject.FindGameObjectWithTag("BestLapTimeYellowText");
        yellowTimeWrapper = GameObject.FindGameObjectWithTag("BestYellowWrapper");

        bestTimeWrapper = GameObject.FindGameObjectWithTag("BestTimeWrapper");
        messageText = GameObject.FindGameObjectWithTag("MessageText");

        DisplayMessageText(startMessage, 10.0f);

        greenTimeWrapper.SetActive(false);
        redTimeWrapper.SetActive(false);
        yellowTimeWrapper.SetActive(false);
        bestTimeWrapper.SetActive(false);

        lapTimerTextWrapper.SetActive(false);
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
            DisplayMessageText("Finish!!");

            bestTimeInCurrentTrackWrapper.SetActive(true);
            UpdateLapTimeIfImproved(lapTime);

            UpdateCurrentTrackBestTime();
        }

        if (other.CompareTag("CheckPoint") && hasStartedLap && !hasPassedCheckPoint)
        {
            hasPassedCheckPoint = true;
            checkPointTime = elapsedTime;
            lapTimerCheckPointText.GetComponent<Text>().text = FormatTime(checkPointTime);
            DisplayMessageText("Halfway!!");

            UpdateCheckPointTimeIfImproved();
        }

        if (other.CompareTag("StartLine") && !hasStartedLap && !hasPassedCheckPoint)
        {
            hasStartedLap = true;

            lapTimerCheckPointText.GetComponent<Text>().text = string.Empty;

            bestTimeInCurrentTrackWrapper.SetActive(false);
            DisplayMessageText("Start!");
            startTime = Time.time;
        }

    }

    private void UpdateCurrentTrackBestTime()
    {
        switch (playerCurrentLocation)
        {
            case PlayerTeleportLocation.FirstTrack:
                bestTimeInCurrentTrackCheckPointText.GetComponent<Text>().text = 
                    bestFirstTrackCheckPointTime == default ? "00:00:00" : FormatTime(bestFirstTrackCheckPointTime);
                bestTimeInCurrentTrackLapTimeText.GetComponent<Text>().text = 
                    bestFirstTrackTime == default ? "00:00:00" : FormatTime(bestFirstTrackTime);
                break;
            case PlayerTeleportLocation.SecondTrack:
                bestTimeInCurrentTrackCheckPointText.GetComponent<Text>().text = 
                    bestSecondTrackCheckPointTime == default ? "00:00:00" : FormatTime(bestSecondTrackCheckPointTime);
                bestTimeInCurrentTrackLapTimeText.GetComponent<Text>().text = 
                    bestSecondTrackTime == default ? "00:00:00" : FormatTime(bestSecondTrackTime);
                break;
            case PlayerTeleportLocation.ThirdTrack:
                bestTimeInCurrentTrackCheckPointText.GetComponent<Text>().text = 
                    bestThirdTrackCheckPointTime == default ? "00:00:00" : FormatTime(bestThirdTrackCheckPointTime);
                bestTimeInCurrentTrackLapTimeText.GetComponent<Text>().text = 
                    bestThirdTrackTime == default ? "00:00:00" : FormatTime(bestThirdTrackTime);
                break;
            default:
                break;
        }
    }

    private void ResetEventHandler()
    {
        elapsedTime = 0f;
        checkPointTime = 0f;
        hasStartedLap = false;
        hasPassedCheckPoint = false;
        lapTimerCheckPointText.GetComponent<Text>().text = string.Empty;
        if (messageText.activeSelf)
        {
            messageText.SetActive(false);
            StopCoroutine(DoDisplayMessageText(default, default));
        }
        if(playerCurrentLocation != PlayerTeleportLocation.MainHub)
        {
            UpdateCurrentTrackBestTime();
            bestTimeInCurrentTrackWrapper.SetActive(true);
        }
    }

    private void MainHubUI()
    {
        lapTimerTextWrapper.SetActive(false);
        if(displayBestTrackTimes)
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
            case PlayerTeleportLocation.SecondTrack:
                if (bestSecondTrackCheckPointTime == default ||
                    checkPointTime < bestSecondTrackCheckPointTime)
                {
                    bestSecondTrackCheckPointTime = checkPointTime;
                    bestCheckPointTimeRedText.GetComponent<Text>().text = FormatTime(bestSecondTrackCheckPointTime);
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
                {
                    displayBestTrackTimes = true;
                    greenTimeWrapper.SetActive(true);
                    StateAndLocatizationEventManager.RaiseOnFirstTrackCleared();
                }
                break;
            case PlayerTeleportLocation.SecondTrack:
                if (bestSecondTrackTime == default ||
                    lapTime < bestSecondTrackTime)
                {
                    bestSecondTrackTime = lapTime;
                    bestLapTimeRedText.GetComponent<Text>().text = FormatTime(bestSecondTrackTime);
                }
                if (!redTimeWrapper.activeSelf)
                {
                    redTimeWrapper.SetActive(true);
                    StateAndLocatizationEventManager.RaiseOnSecondTrackCleared();
                }
                break;
            case PlayerTeleportLocation.ThirdTrack:
                if (bestThirdTrackTime == default ||
                    lapTime < bestThirdTrackTime)
                {
                    bestThirdTrackTime = lapTime;
                    bestLapTimeYellowText.GetComponent<Text>().text = FormatTime(bestThirdTrackTime);
                }
                if (!yellowTimeWrapper.activeSelf)
                {
                    yellowTimeWrapper.SetActive(true);
                    StateAndLocatizationEventManager.RaiseOnThirdTrackCleared();
                }
                break;
            default:
                break;
        }
    }

    private void LocationChangedEventHandler(PlayerTeleportLocation teleportLocation)
    {
        playerCurrentLocation = teleportLocation;

        switch (teleportLocation)
        {
            case PlayerTeleportLocation.MainHub:
                if (AllTracksCleared() && !allTracksClearedEventSent)
                {
                    StateAndLocatizationEventManager.RaiseOnAllTracksCleared();
                    allTracksClearedEventSent = true;
                }
                inMainHub = true;
                MainHubUI();
                break;
            default:
                inMainHub = false;
                TrackUI();
                UpdateCurrentTrackBestTime();
                bestTimeInCurrentTrackWrapper.SetActive(true);
                break;
        }

    }

    private bool AllTracksCleared()
    {
        return greenTimeWrapper.activeSelf &&
               redTimeWrapper.activeSelf &&
               yellowTimeWrapper.activeSelf;
    }

    private void DisplayMessageText(string message, float displayTime = 1.0f)
    {
        if(messageText.activeSelf)
        {
            messageText.SetActive(false);
            StopCoroutine(DoDisplayMessageText(default, default));
        }
        StartCoroutine(DoDisplayMessageText(message, displayTime));
    }

    private IEnumerator DoDisplayMessageText(string message, float displayTime)
    {
        messageText.SetActive(true);
        messageText.GetComponent<Text>().text = message;
        messageTextAnimator.SetTrigger("DisplayMessageText");

        yield return new WaitForSeconds(displayTime);

        messageTextAnimator.SetTrigger("RemoveMessageText");

        yield return new WaitForSeconds(0.25f);

        messageText.SetActive(false);
    }
}
