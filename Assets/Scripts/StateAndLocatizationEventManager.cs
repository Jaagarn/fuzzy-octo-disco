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

}
