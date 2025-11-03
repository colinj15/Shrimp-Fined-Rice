using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class VeggieController : MonoBehaviour
{
    [Header("State / Visuals")]
    private SpriteRenderer sr;
    public float dirtyness = 0.5f; // 0.5 = very dirty, 0.0 = clean

    [Header("Drag Settings")]
    public bool usePhysicsWhileDragging = false; // if true, follow via velocity; else MovePosition
    public float followSpeed = 30f;              // higher = snappier
    public float maxSpeed = 25f;                 // clamp when using velocity mode
    public LayerMask pickMask = ~0;              // which layers are pickable (default = everything)

    private Rigidbody2D rb;
    private Collider2D col;
    private Camera cam;

    private bool isDragging = false;
    private Vector2 dragOffset;          // keeps relative grab point
    private Vector2 targetPos;           // updated each frame from the mouse
    private float grabbedZ;              // depth used for Screen<->World conversion

    private float originalGravityScale;
    private float originalDrag;
    public float dragWhileDragging = 10f; // helps damp motion in velocity mode

    private bool isCleaning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        rb.freezeRotation = true; // prevent spinning

        originalGravityScale = rb.gravityScale;
        originalDrag = rb.linearDamping;
    }

    void Update()
    {
        // Visuals
        sr.color = new Color(1f, 1f - dirtyness, 1f - dirtyness);

        var mouse = Mouse.current;
        if (mouse == null || cam == null) return;

        // Convert mouse to world, keeping the object’s current screen-space depth
        if (!isDragging)
        {
            grabbedZ = cam.WorldToScreenPoint(transform.position).z;
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(new Vector3(
            mouse.position.x.ReadValue(),
            mouse.position.y.ReadValue(),
            grabbedZ
        ));
        targetPos = mouseWorld;

        // Handle press -> start drag if pressed over this collider
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld, pickMask);
            if (hit != null && hit == col)
            {
                isDragging = true;
                dragOffset = (Vector2)transform.position - mouseWorld;

                // Tame physics while dragging
                rb.gravityScale = 0f;
                if (usePhysicsWhileDragging) rb.linearDamping = dragWhileDragging;
                rb.angularVelocity = 0f; // keep from spinning
            }
        }

        // Handle release -> stop drag
        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;
            rb.gravityScale = originalGravityScale;
            rb.linearDamping = originalDrag;
        }
    }

    void FixedUpdate()
    {
        if (!isDragging) return;

        Vector2 desired = targetPos + dragOffset;

        if (usePhysicsWhileDragging)
        {
            // Velocity-based follow for “physical” feel
            Vector2 toTarget = desired - rb.position;
            Vector2 desiredVel = toTarget * followSpeed;                   // proportional controller
            desiredVel = Vector2.ClampMagnitude(desiredVel, maxSpeed);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desiredVel, 0.6f);     // smooth in
        }
        else
        {
            // MovePosition for crisp, stable follow
            Vector2 newPos = Vector2.Lerp(rb.position, desired, 1f - Mathf.Exp(-followSpeed * Time.fixedDeltaTime));
            rb.MovePosition(newPos);
            rb.linearVelocity = Vector2.zero; // prevent residual drift
        }

        // Cleaning logic
        if (isCleaning)
        {
            dirtyness = Mathf.Max(0f, dirtyness - 0.01f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isCleaning = true;
        }

        if (collision.gameObject.CompareTag("Disposal"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isCleaning = false;
        }
    }

    public void setSprite(Sprite newSprite)
    {
        sr.sprite = newSprite;
    }

    void OnDrawGizmosSelected()
    {
        // Helps visualize where we're trying to move while dragging
        if (isDragging)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere((Vector3)(targetPos + dragOffset), 0.15f);
        }
    }
}
