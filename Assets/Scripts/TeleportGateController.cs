using UnityEngine;

public class TeleportGateController : MonoBehaviour
{
    [SerializeField]
    private GameObject firstTrackMarker;
    [SerializeField]
    private GameObject secondTrackMarker;
    [SerializeField]
    private GameObject thirdTrackMarker;

    [SerializeField]
    private GameObject firstTeleportGate;
    [SerializeField]
    private GameObject secondTeleportGate;
    [SerializeField]
    private GameObject thirdTeleportGate;

    [SerializeField]
    private GameObject winningCrown;
    [SerializeField]
    private GameObject arrow;

    private void OnEnable()
    {
        StateAndLocatizationEventManager.OnFirstTrackCleared += FirstTrackClearedEventHandler;
        StateAndLocatizationEventManager.OnSecondTrackCleared += SecondTrackClearedEventHandler;
        StateAndLocatizationEventManager.OnAllTracksCleared += AllTracksClearedEventHandler;
    }

    private void OnDisable()
    {
        StateAndLocatizationEventManager.OnFirstTrackCleared -= FirstTrackClearedEventHandler;
        StateAndLocatizationEventManager.OnSecondTrackCleared -= SecondTrackClearedEventHandler;
        StateAndLocatizationEventManager.OnAllTracksCleared -= AllTracksClearedEventHandler;
    }

    private void Start()
    {
        firstTrackMarker.SetActive(true);
        firstTeleportGate.SetActive(true);
    }

    private void FirstTrackClearedEventHandler()
    {
        secondTrackMarker.SetActive(true);
        secondTeleportGate.SetActive(true);
    }

    private void SecondTrackClearedEventHandler()
    {
        thirdTrackMarker.SetActive(true);
        thirdTeleportGate.SetActive(true);
    }

    private void AllTracksClearedEventHandler()
    {
        winningCrown.SetActive(true);
        arrow.SetActive(true);
    }

}
