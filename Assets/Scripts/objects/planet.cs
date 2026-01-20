using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class planet : MonoBehaviour
{
    [SerializeField] private float gravityStrength;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // 1. Calculate Direction: (Planet Position - Rocket Position)
                Vector2 direction = (transform.position - collision.transform.position).normalized;

                // 2. Calculate Distance for "Inverse Square Law" (Closer = Stronger)
                // We clamp distance so it doesn't get infinitely strong at the center
                float distance = Vector2.Distance(transform.position, collision.transform.position);
                distance = Mathf.Clamp(distance, 1f, 20f);

                // 3. Calculate Force: Strength / Distance
                // (This mimics real gravity: it gets weaker the further away you are)
                float forceMagnitude = gravityStrength / distance;

                // 4. Apply the Non-Contact Force
                Vector2 force = direction * forceMagnitude;
                playerRb.AddForce(force);
            }
        }
    }
}
