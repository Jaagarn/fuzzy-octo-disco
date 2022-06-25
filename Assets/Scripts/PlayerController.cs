using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private GameObject mainCamera;
    private Vector3 currentPlayerResetPosition;
    
    private bool isBreaking = false;
    private bool isControlsDisabled = false;
    private bool isGrounded = true;

    private readonly IEnumerable<KeyValuePair<string, Vector3>> playerPostitionTeleports = new Dictionary<string, Vector3>()
    {
        { "MainHub", new Vector3( 124, 8, -14 ) },
        { "FirstTrack", new Vector3( -4.2f, 2.5f, 2 ) },
        { "FirstTrackSecret", new Vector3( 28f, 5f, 2.6f ) },
        { "ThirdTrack", new Vector3( 198.67f, 5.9f, 96.27f ) }
    };

    // Make event. Lazy
    // This is truly terrible
    public static bool resetCamera = false;
    public static bool resetTimer = false;
    public static bool inMainHub = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        var postition = GetVector3FromString("MainHub");
        playerRb.position = postition;
        currentPlayerResetPosition = postition;
        inMainHub = true;
    }

    void Update()
    {
        if (isControlsDisabled)
            return;

        verticalInput = Input.GetAxis("Vertical");

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isGrounded)
        {
            playerRb.angularDrag = 20.0f;
            isBreaking = true;
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
        {
            playerRb.angularDrag = 0.05f;
            isBreaking = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * 200.0f);
            isGrounded = false;
        }

        if (playerRb.position.y <= -4.0f || Input.GetKeyDown(KeyCode.R))
            FadeUIReset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainHubTeleport"))
            FadeUITeleport(
                teleportTo: "MainHub",
                newResetPostition: true);

        if (other.CompareTag("FirstTrackTeleport"))
            FadeUITeleport(
                teleportTo: "FirstTrack",
                newResetPostition: true);

        if (other.CompareTag("FirstTrackSecret"))
            FadeUITeleport(
                teleportTo: "FirstTrackSecret",
                newResetPostition: false);

        if (other.CompareTag("ThirdTrackTeleport"))
            FadeUITeleport(
                teleportTo: "ThirdTrack",
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

    private void ResetPlayer()
    {
        playerRb.velocity = Vector3.zero;
        resetCamera = true;
        resetTimer = true;
    }

    private Vector3 GetVector3FromString(string teleport)
    {
        return playerPostitionTeleports.Where(p => p.Key.Equals(teleport))
                                       .Select(p => p.Value)
                                       .FirstOrDefault();
    }

    private void FadeUITeleport(
        string teleportTo, 
        bool newResetPostition = false)
    {
        StartCoroutine(DoFadeUITeleport(teleportTo, newResetPostition));
    }

    private void FadeUIReset()
    {
        StartCoroutine(DoFadeUIReset());
    }

    private IEnumerator DoFadeUITeleport(string teleportTo, bool newResetPostition)
    {
        isControlsDisabled = true;
        teleportUIAnimator.SetTrigger("StartTeleport");

        yield return new WaitForSeconds(0.5f);

        var vector3NewPosition = GetVector3FromString(teleportTo);
        playerRb.position = vector3NewPosition;
        ResetPlayer();
        inMainHub = string.Equals(teleportTo, "MainHub");

        if (newResetPostition)
            currentPlayerResetPosition = vector3NewPosition;

        teleportUIAnimator.SetTrigger("FinishTeleport");

        yield return new WaitForSeconds(0.5f);

        isControlsDisabled = false;
    }

    private IEnumerator DoFadeUIReset()
    {
        isControlsDisabled = true;
        teleportUIAnimator.SetTrigger("StartTeleport");

        yield return new WaitForSeconds(0.5f);

        playerRb.position = currentPlayerResetPosition;
        ResetPlayer();

        teleportUIAnimator.SetTrigger("FinishTeleport");

        yield return new WaitForSeconds(0.5f);

        isControlsDisabled = false;
    }
}
