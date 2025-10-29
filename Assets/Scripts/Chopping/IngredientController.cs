using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public float spawnLoc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnLoc = Random.Range(-7f, 7f);
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(spawnLoc, speed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
