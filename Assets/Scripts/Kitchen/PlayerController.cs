using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    public float speed = 3f;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Centers the player in their starting cell
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 worldCenter = grid.GetCellCenterWorld(cellPos);
        transform.position = new Vector3(worldCenter.x, worldCenter.y, transform.position.z);

        targetPosition = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Sets the targetPosition to the center of the clicked cell
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);
            Vector3 worldCenter = grid.GetCellCenterWorld(cellPos);

            targetPosition = new Vector2(worldCenter.x, worldCenter.y);
            moving = true;
        }
    }

    void FixedUpdate()
    {
       if (moving)
        {
            // Moves the player until it reaches the correct cell
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Mathf.Abs(rb.position.x - targetPosition.x) < 0.01f)
                moving = false;
        }
    }
}
