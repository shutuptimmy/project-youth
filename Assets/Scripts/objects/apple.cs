using UnityEngine;

public class apple : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveSpeed;
    private float rotateSpeed;
    // private float timeLimit = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void setup(float speed)
    {
        moveSpeed = speed;
        rotateSpeed = Random.Range(-100f, 100f);
    }

    private void Update()
    {
        rb.velocity = Vector2.down * moveSpeed;
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ground"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
