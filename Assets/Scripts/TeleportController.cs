using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeleportLocation
{
    MainHub,
    FirstTrack,
    FirstTrackSecret,
    SecondTrack,
    ThirdTrack
}

public class TeleportController : MonoBehaviour
{
    public static readonly IEnumerable<KeyValuePair<PlayerTeleportLocation, Vector3>> playerPostitionTeleports = new Dictionary<PlayerTeleportLocation, Vector3>()
    {
        { PlayerTeleportLocation.MainHub, new Vector3( 124, 8, -14 ) },
        { PlayerTeleportLocation.FirstTrack, new Vector3( -4.2f, 2.5f, 2 ) },
        { PlayerTeleportLocation.FirstTrackSecret, new Vector3( 28f, 7f, 2.6f ) },
        { PlayerTeleportLocation.SecondTrack, new Vector3( 125.4f, 19f, 77.5f ) },
        { PlayerTeleportLocation.ThirdTrack, new Vector3( 198.67f, 5.9f, 96.27f ) }
    };

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
