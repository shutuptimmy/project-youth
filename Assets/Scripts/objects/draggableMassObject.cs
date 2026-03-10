using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class draggableMassObject : MonoBehaviour
{
    [SerializeField] private desk deskScript;
    [SerializeField] private float weight;
    private Rigidbody2D rb;
    public bool isDragging { get; private set; } = false;


    // Track if the velocity is low enough to count as "Placed"
    public bool isStable => rb.velocity.magnitude < 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weight = rb.mass;
    }


    public void setDragging(bool dragging)
    {
        isDragging = dragging;

        // Optional: reduce gravity while dragging to make it feel easier to lift
        rb.gravityScale = dragging ? 0.5f : 1f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MassObj"))
        {
            // 2. Determine who reports the damage to prevent double counting.
            // Logic: The object ON THE BOTTOM reports the damage (it 'absorbs' the hit).
            // If I am below the other object...
            if (this.transform.position.y < collision.transform.position.y)
            {
                if (deskScript != null && deskScript.isObjectResting(this.rb))
                {
                    float impactForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
                    deskScript.applyImpactDamage(impactForce);
                }
            }
        }
    }
}
