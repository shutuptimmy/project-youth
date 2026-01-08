using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class pushableBox : InteractableBase, IDraggable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D mainCollider;
    private SpriteRenderer spriteRenderer;
    private FixedJoint2D grabJoint;
    private string boxId;
    private bool isDragging = false;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
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

    private void Grab()
    {
        isDragging = true;
        gameEventsManager.instance.miscEvents.boxDraggingStateChanged(boxId, true);
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

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
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Destroy the joint to disconnect
        if (grabJoint != null)
        {
            Destroy(grabJoint);
        }
    }
    public bool IsCurrentlyDragging()
    {
        return isDragging;
    }

    public string setPuzzleId(string id)
    {
        return boxId = id;
    }
    public BoxCollider2D GetCollider2D()
    {
        return mainCollider;
    }
}
