using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Apple Minigame Manager")]
    [SerializeField] fallingApplesManager fallingApplesManager;
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isInvincible = false;

    // main components
    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private const string horizontal = "horizontal";
    private const string vertical = "vertical";
    private const string lastHorizontal = "lastHorizontal";
    private const string lastVertical = "lastVertical";

    private int playerGender;
    private float moveSpeed = 1f;
    private bool movementDisabled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        gameEventsManager.instance.inputEvents.onMovePressed += MovePressed;
        gameEventsManager.instance.inputEvents.onSubmitPressed += performDash;
        gameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        gameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
        animator.SetInteger("gender", playerGender);
    }

    private void OnDestroy()
    {
        gameEventsManager.instance.inputEvents.onMovePressed -= MovePressed;
        gameEventsManager.instance.playerEvents.onDisablePlayerMovement -= DisablePlayerMovement;
        gameEventsManager.instance.playerEvents.onEnablePlayerMovement -= EnablePlayerMovement;
    }

    private void DisablePlayerMovement()
    {
        movementDisabled = true;
        // also ensure we stop any current movement
        velocity = Vector2.zero;
    }

    private void EnablePlayerMovement()
    {
        movementDisabled = false;
    }

    private void MovePressed(Vector2 moveDir)
    {
        velocity = moveDir.normalized * moveSpeed;

        if (movementDisabled) velocity = Vector2.zero;
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.velocity = velocity;
    }

    private void UpdateAnimations()
    {
        // handles sprite animation
        animator.SetFloat(horizontal, velocity.x);
        animator.SetFloat(vertical, velocity.y);

        if (velocity != Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, velocity.x);
            animator.SetFloat(lastVertical, velocity.y);
        }

        if (rb.velocity.x < 0) sprite.flipX = true;
        else if (rb.velocity.x > 0) sprite.flipX = false;
    }

    // for tug of war
    public void setAnimation(int status)
    {
        string spriteGender = playerGender == 0 ? "Boy" : "Girl";
        Debug.Log("SpriteGender: " + spriteGender + ". PlayerGender: " + playerGender);

        switch (status)
        {
            // Move
            case 0:
                animator.Play("player" + spriteGender + "SideWalk");
                break;
            // Idle
            case 1:
                animator.Play("player" + spriteGender + "SideIdle");
                break;
            default:
                Debug.Log("setAnimation out of bounds: " + status);
                break;
        }
    }

    // for dodge apples
    private IEnumerator playerDash()
    {
        canDash = false;
        isDashing = true;

        // apples go through the player's head while dashing instead of hiding it
        gameObject.layer = LayerMask.NameToLayer("playerInvincible");
        isInvincible = true;

        Vector2 dashDir = velocity.normalized;
        rb.velocity = dashDir * dashSpeed;

        sprite.color = new Color(1, 1, 1, 0.7f);
        dashTrail.emitting = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        gameObject.layer = LayerMask.NameToLayer("player");
        isInvincible = false;

        dashTrail.emitting = false;

        float timer = 0f;
        while (timer < dashCooldown)
        {
            timer += Time.deltaTime;
            // Lerp opacity
            float alpha = Mathf.Lerp(0.7f, 1f, timer / dashCooldown);
            sprite.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        sprite.color = Color.white;
        canDash = true;
    }

    void performDash(inputEventContext inputEventContext)
    {
        if (!inputEventContext.Equals(inputEventContext.MINIGAME) && fallingApplesManager == null) return;

        if (isDashing || !canDash || velocity == Vector2.zero) return;
        Debug.Log("Dash");
        StartCoroutine(playerDash());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If we hit an apple and we aren't invincible
        if (collider.GetComponent<apple>() != null)
        {
            if (!isInvincible)
            {
                gameEventsManager.instance.miscEvents.playerTookDamage();
                StartCoroutine(InvincibilityRoutine());
            }
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        gameObject.layer = LayerMask.NameToLayer("playerInvincible");
        isInvincible = true;

        float duration = 2f;
        float blinkInterval = 0.2f;
        float endTime = Time.time + duration;

        // Blinking Effect
        while (Time.time < endTime)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        sprite.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("player");
        isInvincible = false;
    }

    public void loadData(gameData data)
    {
        this.transform.position = data.playerPosition;
        this.playerGender = data.playerGender;
    }

    public void saveData(gameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
