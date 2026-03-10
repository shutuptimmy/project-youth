using UnityEngine;
using UnityEngine.InputSystem;

public class dragDropController : MonoBehaviour
{
    [Header("Manager Script")]
    [SerializeField] private fragileDeskManager manager;
    [Header("Controller Config")]
    [SerializeField] private float springFrequency; // Higher = Stiffer (Sticks closer to mouse)
    [SerializeField] private float springDamping; // Oscillation (bouncing effect while holding)
    [SerializeField] private float shakeIntensity; // How far it jitters
    [SerializeField] private float shakeSpeed;      // How fast it jitters

    [Header("Rotation Stabilization")]
    [SerializeField] private float holdDrag; // How hard it tries to stay straight
    [SerializeField] private float uprightForce;

    private draggableMassObject currentObject;
    private Vector3 mousePosOffset;
    private Rigidbody2D objRb;

    // restore object's current properties
    // private float originalDrag;

    private SpringJoint2D sj;
    private Rigidbody2D rb;
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        sj = GetComponent<SpringJoint2D>();
        sj.frequency = springFrequency;
        sj.dampingRatio = springDamping;
    }

    void Start()
    {
        gameEventsManager.instance.inputEvents.OnDragPressed += onObjectSelected;
        gameEventsManager.instance.inputEvents.OnDragReleased += onObjectRelease;
    }

    void OnDestroy()
    {
        gameEventsManager.instance.inputEvents.OnDragPressed -= onObjectSelected;
        gameEventsManager.instance.inputEvents.OnDragReleased -= onObjectRelease;
    }

    void FixedUpdate()
    {
        // Move the hand to the mouse position
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 mousePos = mainCam.ScreenToWorldPoint(screenPos);

        if (currentObject != null)
        {
            // Perlin Noise creates smooth random movement (like a trembling hand)
            float noiseX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f;

            Vector2 shakeOffset = new Vector2(noiseX, noiseY) * shakeIntensity;
            mousePos += shakeOffset;
            stabilizeRotation();
        }
        // shake the mouse pos
        rb.MovePosition(mousePos);
    }

    void onObjectSelected()
    {
        if (!manager.isGameActive) return;
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("MassObj"))
        {
            draggableMassObject item = hit.collider.GetComponent<draggableMassObject>();
            mousePosOffset = item.gameObject.transform.position - mousePos;

            // if (item != null)
            // {
                item.transform.position = mousePos + mousePosOffset;

                currentObject = item;
                currentObject.setDragging(true);

                objRb = hit.collider.attachedRigidbody;


                // save original properties
                // originalDrag = objRb.drag;

                // set properties from inspector
                // objRb.drag = holdDrag;

                // objRb.rotation = 0f; // TODO: lerp instead of instant rotation to prevent clipping
                // objRb.angularVelocity = 0f;
                // objRb.transform.rotation = Quaternion.identity;

                Debug.Log("Draggin" + currentObject.name);


                // Connect the spring
                sj.connectedBody = objRb;
                // connect to Center of object (Vector2.zero) so it centers on mouse
                // sj.connectedAnchor = Vector2.zero;
                sj.enabled = true;
            // }
        }
    }

    void onObjectRelease()
    {
        if (currentObject != null)
        {
            Debug.Log("Releasin");
            currentObject.setDragging(false);

            // Restore the object's original physics properties
            currentObject = null;

            // objRb.drag = originalDrag;
            objRb = null;
        }
        sj.enabled = false;
        sj.connectedBody = null;
    }

    void stabilizeRotation()
    {
        // Calculate the shortest distance to 0 degrees
        // deltaAngle handles the wrap-around logic (e.g. 350 degrees is -10 degrees)
        float angleDifference = Mathf.DeltaAngle(objRb.rotation, 0f);

        // Apply Torque: Force * Angle * Mass (so heavy objects stabilize just as well as light ones)
        float torque = angleDifference * uprightForce; // * objRb.mass;

        objRb.AddTorque(torque * Time.fixedDeltaTime);
    }
}
