using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerTeleportLocation
{
    MainHub,
    FirstTrack,
    FirstTrackSecret,
    ThirdTrack
}

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Animator teleportUIAnimator;

    private const float speed = 10.0f;
    private const float maxSpeed = 160.0f;
    private float speedModifier = 1.0f;
    private float verticalInput;

    private Rigidbody playerRb;
    private GameObject mainCamera;
    private GameObject uiFadeToBlack;
    private PlayerTeleportLocation currentPlayerResetLocation;

    private bool isBreaking = false;
    private bool isControlsDisabled = false;
    private bool isGrounded = true;
    private bool gameIsPaused = false;

    private readonly IEnumerable<KeyValuePair<PlayerTeleportLocation, Vector3>> playerPostitionTeleports = new Dictionary<PlayerTeleportLocation, Vector3>()
    {
        { PlayerTeleportLocation.MainHub, new Vector3( 124, 8, -14 ) },
        { PlayerTeleportLocation.FirstTrack, new Vector3( -4.2f, 2.5f, 2 ) },
        { PlayerTeleportLocation.FirstTrackSecret, new Vector3( 28f, 7f, 2.6f ) },
        { PlayerTeleportLocation.ThirdTrack, new Vector3( 198.67f, 5.9f, 96.27f ) }
    };

    private void OnEnable()
    {
        StateAndLocatizationEventManager.OnGamePaused += GamePausedHandler;
        StateAndLocatizationEventManager.OnGameResumed += GameResumedHandler;
    }

    private void OnDisable()
    {

        StateAndLocatizationEventManager.OnGamePaused += GamePausedHandler;
        StateAndLocatizationEventManager.OnGameResumed += GameResumedHandler;
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        uiFadeToBlack = GameObject.FindGameObjectWithTag("UIFadeToBlack");
        uiFadeToBlack.SetActive(false);
        var postition = GetVector3FromPlayerTeleportLocation(PlayerTeleportLocation.MainHub);
        playerRb.position = postition;
        currentPlayerResetLocation = PlayerTeleportLocation.MainHub;
    }

    private void Update()
    {
        if (isControlsDisabled || gameIsPaused)
            return;

        verticalInput = Input.GetAxis("Vertical");

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isGrounded)
        {
            playerRb.angularDrag = 20.0f;
            isBreaking = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            playerRb.angularDrag = 0.05f;
            isBreaking = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * 200.0f);
            isGrounded = false;
        }

        if (playerRb.transform.position.y <= -4.0f || Input.GetKeyDown(KeyCode.R))
        {
            FadeUITeleport(
                teleportLocation: currentPlayerResetLocation);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainHubTeleport"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.MainHub,
                newResetPostition: true);

        if (other.CompareTag("FirstTrackTeleport"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.FirstTrack,
                newResetPostition: true);

        if (other.CompareTag("FirstTrackSecret"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.FirstTrackSecret);

        if (other.CompareTag("ThirdTrackTeleport"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.ThirdTrack,
                newResetPostition: true);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Fetch parent of colliding gameObject to fetch data from parentSlideController
        GameObject collidingObjectParent = collision.gameObject.transform.parent.gameObject;
        if (collidingObjectParent.tag.Equals("Slide"))
            speedModifier = collidingObjectParent.GetComponent<ParentSlideController>().speedModifier;
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        speedModifier = 1.0f;
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        if (!isBreaking && isGrounded && !(playerRb.velocity.magnitude >= maxSpeed))
        {
            // Create new vector3 from camera vector3 without rotation
            Vector3 moveDirectionVector = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z);
            Vector3 verticallVector = moveDirectionVector * verticalInput * speed * speedModifier;
            playerRb.AddForce(verticallVector);
        }
    }

    private Vector3 GetVector3FromPlayerTeleportLocation(PlayerTeleportLocation teleportLocation)
    {
        return playerPostitionTeleports.Where(p => p.Key.Equals(teleportLocation))
                                       .Select(p => p.Value)
                                       .FirstOrDefault();
    }

    /// <summary>
    /// A metod that teleports the player to a new location. Activates/deactivates the ui animation that handels fade to black
    /// and runs it. 
    /// </summary>
    /// <param name="teleportTo">
    /// Vector3 for the new position
    /// </param>
    /// <param name="teleportString">
    /// Set teleportString if you want to set inMainHub bool. If you don't give it a value, isMainHub will be false.
    /// </param>
    /// <param name="newResetPostition">
    /// If you want the teleportTo Vector3 to be the new resetPosition 
    /// </param>
    private void FadeUITeleport(
        PlayerTeleportLocation teleportLocation,
        bool newResetPostition = false)
    {
        StartCoroutine(DoFadeUITeleport(teleportLocation, newResetPostition));
    }

    private IEnumerator DoFadeUITeleport(PlayerTeleportLocation teleportLocation, bool newResetPostition)
    {
        var teleportLocationVector3 = GetVector3FromPlayerTeleportLocation(teleportLocation);
        isControlsDisabled = true;
        uiFadeToBlack.SetActive(true);
        teleportUIAnimator.SetTrigger("StartTeleport");

        yield return new WaitForSeconds(0.5f);

        playerRb.position = teleportLocationVector3;
        playerRb.velocity = Vector3.zero;

        StateAndLocatizationEventManager.RaiseOnReset();

        if (newResetPostition)
        {
            currentPlayerResetLocation = teleportLocation;
            StateAndLocatizationEventManager.RaiseOnLocationChange(teleportLocation);
        }

        teleportUIAnimator.SetTrigger("FinishTeleport");

        yield return new WaitForSeconds(0.5f);

        uiFadeToBlack.SetActive(false);
        isControlsDisabled = false;
    }

    private void GamePausedHandler() { gameIsPaused = true; }
    private void GameResumedHandler() { gameIsPaused = false; }

}
