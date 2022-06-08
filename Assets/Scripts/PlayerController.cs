using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly IEnumerable<KeyValuePair<string, Vector3>> playerPostitionTeleports = new Dictionary<string, Vector3>()
    {
        { "MainHub", new Vector3( 124, 8, -14 ) },
        { "FirstTrack", new Vector3( -4.2f, 2.5f, 2 ) },
        { "FirstTrackSecret", new Vector3( 28f, 5f, 2.6f ) }
    };

    private const float speed = 10.0f;
    private const float maxSpeed = 160.0f;
    private Rigidbody playerRb;
    private GameObject mainCamera;
    private Vector3 currentPlayerResetPosition;
    private float verticalInput;
    private bool isBreaking = false;
    private bool isGrounded = true;
    private bool disableControls = false;
    private float speedModifier = 1.0f;

    [SerializeField]
    private Animator teleportUIAnimator; 

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
        if (disableControls)
            return;

        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            playerRb.angularDrag = 15.0f;
            isBreaking = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
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
        {
            FadeUIReset();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainHubTeleport"))
            FadeUITeleport(
                teleportTo: "MainHub",
                newResetPostition: true, 
                enteringMainHub: true);

        if (other.CompareTag("FirstTrackTeleport"))
            FadeUITeleport(
                teleportTo: "FirstTrack",
                newResetPostition: true,
                enteringMainHub: false);

        if (other.CompareTag("FirstTrackSecret"))
            FadeUITeleport(
                teleportTo: "FirstTrackSecret",
                newResetPostition: false,
                enteringMainHub: false);
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
        bool newResetPostition = false,
        bool enteringMainHub = false)
    {
        StartCoroutine(DoFadeUITeleport(teleportTo, newResetPostition, enteringMainHub));
    }

    private void FadeUIReset()
    {
        StartCoroutine(DoFadeUIReset());
    }

    private IEnumerator DoFadeUITeleport(string teleportTo, bool newResetPostition, bool enteringMainHub)
    {
        disableControls = true;
        teleportUIAnimator.SetTrigger("StartTeleport");

        yield return new WaitForSeconds(0.5f);

        var vector3NewPosition = GetVector3FromString(teleportTo);
        playerRb.position = vector3NewPosition;
        ResetPlayer();
        inMainHub = enteringMainHub;

        if (newResetPostition)
            currentPlayerResetPosition = vector3NewPosition;

        teleportUIAnimator.SetTrigger("FinishTeleport");

        yield return new WaitForSeconds(0.5f);

        disableControls = false;
    }

    private IEnumerator DoFadeUIReset()
    {
        disableControls = true;
        teleportUIAnimator.SetTrigger("StartTeleport");

        yield return new WaitForSeconds(0.5f);

        playerRb.position = currentPlayerResetPosition;
        ResetPlayer();

        teleportUIAnimator.SetTrigger("FinishTeleport");

        yield return new WaitForSeconds(0.5f);

        disableControls = false;
    }
}
