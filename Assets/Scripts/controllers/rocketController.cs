using UnityEngine;
using UnityEngine.UI;

public class rocketController : MonoBehaviour
{
    [Header("Minigame Manager")]
    [SerializeField] private backToEarthManager manager;

    [Header("Rocket Config")]
    [SerializeField] private float initialKickForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private BoxCollider2D landCollider;

    [Header("Fuel Config")]
    [SerializeField] private float maxFuel;
    private float rocketFuel;
    [SerializeField] private float steeringConsumptionPerSecond;
    [SerializeField] private float boostForce;
    [SerializeField] private float boostConsumptionPerSecond;
    [SerializeField] private float boostRechargePerSecond;
    [SerializeField] private Slider boostBarUI;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private float currentRotationZ = -90f;

    private bool isSteering = false;
    private bool isBoosting = false;
    private bool hasCrashed = false;
    private bool movementDisabled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rocketFuel = maxFuel;
    }

    void Start()
    {
        gameEventsManager.instance.inputEvents.onMovePressed += MovePressed;
        gameEventsManager.instance.inputEvents.onSubmitPressed += performBoost;
        gameEventsManager.instance.inputEvents.onSubmitReleased += releaseBoost;
        gameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        gameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
    }

    void OnDestroy()
    {
        gameEventsManager.instance.inputEvents.onMovePressed -= MovePressed;
        gameEventsManager.instance.inputEvents.onSubmitPressed -= performBoost;
        gameEventsManager.instance.inputEvents.onSubmitReleased -= releaseBoost;
        gameEventsManager.instance.playerEvents.onDisablePlayerMovement -= DisablePlayerMovement;
        gameEventsManager.instance.playerEvents.onEnablePlayerMovement -= EnablePlayerMovement;
    }

    void Update()
    {
        handleAnimation();

        if (!movementDisabled && !hasCrashed)
        {
            handleFuel();
            handleSteering();
        }
    }

    void FixedUpdate()
    {
        if (!movementDisabled && !hasCrashed)
        {
            handleBoost();
        }
    }

    void DisablePlayerMovement()
    {
        movementDisabled = true;
        // also ensure we stop any current movement
        moveInput = Vector2.zero;
        isBoosting = false;
        isSteering = false;
    }

    void EnablePlayerMovement()
    {
        movementDisabled = false;
    }

    void MovePressed(Vector2 moveDir)
    {
        moveInput = moveDir;
        isSteering = moveInput.x != 0 && rocketFuel > 0;
    }

    void performBoost(inputEventContext context)
    {
        if (rocketFuel > 0) isBoosting = true;
    }

    void releaseBoost(inputEventContext context)
    {
        isBoosting = false;
    }

    void handleBoost()
    {
        if (isBoosting && rocketFuel > 0)
        {
            // This applies force exactly where the head is pointing
            rb.AddForce(transform.up * boostForce);
        }

        // clamp the speed so it doesn't go beyond limits
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void handleSteering()
    {
        if (isSteering && rocketFuel > 0)
        {
            float rotationAmount = -moveInput.x * turnSpeed * Time.deltaTime;
            currentRotationZ += rotationAmount;
        }
        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
    }

    void handleFuel()
    {
        if (isBoosting && rocketFuel > 0) rocketFuel -= boostConsumptionPerSecond * Time.deltaTime;
        else if (isSteering && rocketFuel > 0) rocketFuel -= steeringConsumptionPerSecond * Time.deltaTime;
        else
        {
            isBoosting = false;
            isSteering = false;
            rocketFuel += boostRechargePerSecond * Time.deltaTime;
        }

        boostBarUI.value = rocketFuel / maxFuel;
    }

    void handleAnimation()
    {
        bool moving = (isBoosting || isSteering) && rocketFuel > 0;
        animator.SetBool("isMoving", moving);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCrashed) return; // Ignore collisions if already crashed

        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Earth"))
        {
            if (collision.otherCollider == landCollider)
            {
                Debug.Log("Landed safely with Exhaust!");
                manager.MinigameComplete(true);
                DisablePlayerMovement();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                // We hit Earth with the nose/body -> Crash!
                Debug.Log("Crashed nose-first into Earth!");
                rocketCrashed();
            }
        }
        else rocketCrashed();
    }

    void rocketCrashed()
    {
        gameEventsManager.instance.playerEvents.playerTookDamage();

        hasCrashed = true;
        isBoosting = false;
        isSteering = false;

        animator.SetBool("hasCrashed", true);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void restartRocket()
    {
        rocketFuel = maxFuel;
        hasCrashed = false;
        animator.SetBool("hasCrashed", false);
        rb.constraints = ~RigidbodyConstraints2D.FreezeAll;

        // reset transform
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        currentRotationZ = -90f;
        this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, currentRotationZ));

        this.rb.AddForce(Vector2.right * initialKickForce, ForceMode2D.Impulse);
    }
}
