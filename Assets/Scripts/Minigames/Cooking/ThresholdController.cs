using UnityEngine;

public class ThresholdController : MonoBehaviour
{
    private SpriteRenderer top;
    private SpriteRenderer bottom;
    private int dir = 1; // Direction of movement: 1 for up, -1 for down
    private bool spdChanged = false; // Has speed been changed this second
    public float spd; // Speed of threshold movement
    public CookManager cookManager; // Reference to CookManager
    public float minSpd;
    public float maxSpd;
    void Start()
    {
        top = transform.Find("Top").GetComponent<SpriteRenderer>();
        bottom = transform.Find("Bottom").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the threshold up and down
        transform.position += new Vector3(0, dir * spd * Time.deltaTime, 0);

        // Change speed randomly every 2 seconds
        if (cookManager.GetTime() % 2 == 0 && !spdChanged)
        {
            spd = Random.Range(minSpd, maxSpd);
            spdChanged = true;
        }

        if (cookManager.GetTime() % 2 != 0 && spdChanged)
        {
            spdChanged = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            dir *= -1;
        }
    }
}
