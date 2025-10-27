using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

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
        gameEventsManager.instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
        gameEventsManager.instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
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

        if (movementDisabled)
        {
            velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void UpdateAnimations()
    {
        // handles sprite animation
        animator.SetFloat("speed", (rb.velocity != Vector2.zero) ? 1 : 0);
        if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else if (rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }
    }

    // private void FixedUpdate()
    // {
    //     HandleHorizontalMovement();
    // }

    // private void HandleHorizontalMovement()
    // {
    //     Vector2 movement = inputManager.GetInstance().GetMoveDirection();

    //     // stops the movement if dialogue is playing
    //     // if (dialogueManager.GetInstance().isDialoguePlayin)
    //     // {
    //     //     movement = Vector2.zero;
    //     // }

    //     rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * (moveSpeed / 2));



    //     // handles sprite animation
    //     animator.SetFloat("speed", (rb.velocity != Vector2.zero) ? 1 : 0);
    //     if (rb.velocity.x < 0)
    //     {
    //         sprite.flipX = true;
    //     }
    //     else if (rb.velocity.x > 0)
    //     {
    //         sprite.flipX = false;
    //     }
    // }
}
