using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class sortingBox : InteractableBase, IDraggable
{
    [Header("Components")]
    public boxDataSO data;
    [SerializeField] private sortingBoxesManager manager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D mainCollider;

    private SpriteRenderer spriteRenderer;
    private FixedJoint2D grabJoint;

    private string boxId;
    private bool isDragging = false;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // rb = GetComponent<Rigidbody2D>();

        // Generate a random ID
        boxId = "sortingBox_" + GetInstanceID();
    }
    public override void Interact()
    {
        if (isDragging)
        {
            Release();
            Debug.Log("Release!");
        }
        else
        {
            Grab();
            Debug.Log("Grab!");
        }
    }

    public void Grab()
    {
        isDragging = true;
        gameEventsManager.instance.miscEvents.boxDraggingStateChanged(boxId, true);
        // rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;

        // Show UI and its data
        manager.showBoxDetails(data);

        // Create a joint to connect Box to Player
        grabJoint = gameObject.AddComponent<FixedJoint2D>();
        grabJoint.connectedBody = player.GetComponent<Rigidbody2D>();
        grabJoint.dampingRatio = 1f;
        grabJoint.frequency = 0;
    }

    public void Release()
    {
        isDragging = false;
        gameEventsManager.instance.miscEvents.boxDraggingStateChanged(boxId, false);
        // rb.constraints = RigidbodyConstraints2D.FreezeAll;

        manager.hideBoxDetails();

        // Destroy the joint to disconnect
        Destroy(grabJoint);

        // Restore Collision
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Physics2D.IgnoreCollision(mainCollider, playerCollider, false);

            // --- THE FIX: Double Check Range ---
            StartCoroutine(CheckRangeAfterRelease(playerCollider));
        }
    }

    public void forceReset()
    {
        // Ensure dragging is false
        isDragging = false;

        // Ensure constraints are reset to "Sitting on the floor" mode
        // if (rb != null) rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Ensure joint is gone
        if (grabJoint != null) Destroy(grabJoint);

        // Ensure collision with player is allowed again
        if (player != null)
        {
            Physics2D.IgnoreCollision(mainCollider, player.GetComponent<Collider2D>(), false);
        }

        // Force the base class state to "Clean"
        isInteractable = false;
        // if (interactableVisualCue != null)
        // {
        interactableVisualCue.SetActive(false);
        // }
    }

    IEnumerator CheckRangeAfterRelease(Collider2D playerCollider)
    {
        // Wait for the next fixed physics update so the 'IgnoreCollision' change registers
        yield return new WaitForFixedUpdate();

        // Now ask: "Is the player actually touching this box?"
        if (mainCollider != null && !mainCollider.IsTouching(playerCollider))
        {
            // If not touching, force interactable off
            isInteractable = false;
            // if (interactableVisualCue != null)
            // {
            interactableVisualCue.SetActive(false);
            // }
        }
    }

    public bool IsCurrentlyDragging()
    {
        return isDragging;
    }

    public BoxCollider2D GetCollider2D()
    {
        return mainCollider;
    }

}
