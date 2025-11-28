using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
// [RequireComponent(typeof(SpriteRenderer))]
public class pushableBox : InteractableBase
{
    // private SpriteRenderer spriteRenderer;
    private FixedJoint2D grabJoint;
    private bool isDragging = false;

    // void Reset()
    // {
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    // }

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

        // Create a joint to connect Box to Player
        grabJoint = gameObject.AddComponent<FixedJoint2D>();
        grabJoint.connectedBody = player.GetComponent<Rigidbody2D>();
        grabJoint.dampingRatio = 1f;
        grabJoint.frequency = 0; // Rigid connection
    }

    private void Release()
    {
        isDragging = false;

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
}
