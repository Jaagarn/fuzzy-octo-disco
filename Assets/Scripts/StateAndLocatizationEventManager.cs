using UnityEngine;

public class StateAndLocatizationEventManager : MonoBehaviour
{
    public delegate void Reset();
    public static event Reset OnReset;

    public delegate void GamePaused();
    public static event GamePaused OnGamePaused;

    public delegate void GameResumed();
    public static event GameResumed OnGameResumed;

    public delegate void LocationChange(PlayerTeleportLocation teleportLocation);
    public static event LocationChange OnLocationChange;

    public delegate void FirstTrackCleared();
    public static event FirstTrackCleared OnFirstTrackCleared;

    public delegate void SecondTrackCleared();
    public static event SecondTrackCleared OnSecondTrackCleared;

    public delegate void ThirdTrackCleared();
    public static event ThirdTrackCleared OnThirdTrackCleared;

    public delegate void AllTracksCleared();
    public static event AllTracksCleared OnAllTracksCleared;

    public static void RaiseOnReset()
    {
        OnReset?.Invoke();
    }

    public static void RaiseOnGamePaused()
    {
        OnGamePaused?.Invoke();
    }
    public static void RaiseOnGameResumed()
    {
        OnGameResumed?.Invoke();
    }

    public static void RaiseOnLocationChange(PlayerTeleportLocation teleportLocation)
    {
        OnLocationChange?.Invoke(teleportLocation);
    }

    public static void RaiseOnFirstTrackCleared()
    {
        OnFirstTrackCleared?.Invoke();
    }

    public static void RaiseOnSecondTrackCleared()
    {
        OnSecondTrackCleared?.Invoke();
    }

    public static void RaiseOnThirdTrackCleared()
    {
        OnThirdTrackCleared?.Invoke();
    }

    public static void RaiseOnAllTracksCleared()
    {
        OnAllTracksCleared?.Invoke();
    }

}
