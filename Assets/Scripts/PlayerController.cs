using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Animator teleportUIAnimator;

    private const float speed = 10.0f;
    private const float maxSpeed = 160.0f;
    private float speedModifier = 1.0f;
    private float verticalInput;

    private Rigidbody playerRb;
    private GameObject cameraHolder;
    private GameObject uiFadeToBlack;
    private PlayerTeleportLocation currentPlayerResetLocation;

    private bool isBreaking = false;
    private bool isControlsDisabled = false;
    private bool isGrounded = true;
    private bool gameIsPaused = false;

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
        cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        uiFadeToBlack = GameObject.FindGameObjectWithTag("UIFadeToBlack");
        var postition = TeleportController.GetVector3FromPlayerTeleportLocation(PlayerTeleportLocation.MainHub);
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

        if (other.CompareTag("SecondTrackTeleport"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.SecondTrack,
                newResetPostition: true);

        if (other.CompareTag("ThirdTrackTeleport"))
            FadeUITeleport(
                teleportLocation: PlayerTeleportLocation.ThirdTrack,
                newResetPostition: true);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Fetch parent of colliding gameObject to fetch data from parentSlideController
        GameObject collidingObjectParent = null;
        if (collision.gameObject.transform.parent != null)
            collidingObjectParent = collision.gameObject.transform.parent.gameObject;
        if (collidingObjectParent != null && collidingObjectParent.tag.Equals("Slide"))
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
            var verticalVector = cameraHolder.transform.forward * verticalInput * speed * speedModifier;
            playerRb.AddForce(verticalVector);
        }
    }

    /// <summary>
    /// A metod that teleports the player to a new location. Activates/deactivates the ui animation that handels fade to black
    /// and runs it. Sends out Reset and LocationChanged events. But only LocationChanged event IF you put newResetLocation
    /// to true. Otherwise it is assumed you teleport to a "sub-location" contained within the reset location.
    /// </summary>
    /// <param name="teleportLocation">
    /// PlayerTeleportLocation enum for the new position.
    /// </param>
    /// <param name="newResetPostition">
    /// If you want the teleportLocation PlayerTeleportLocation to be the new resetPosition 
    /// </param>
    private void FadeUITeleport(
        PlayerTeleportLocation teleportLocation,
        bool newResetPostition = false)
    {
        StartCoroutine(DoFadeUITeleport(teleportLocation, newResetPostition));
    }

    private IEnumerator DoFadeUITeleport(PlayerTeleportLocation teleportLocation, bool newResetPostition)
    {
        var teleportLocationVector3 = TeleportController.GetVector3FromPlayerTeleportLocation(teleportLocation);
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
