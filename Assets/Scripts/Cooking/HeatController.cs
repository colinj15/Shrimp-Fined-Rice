using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HeatController : MonoBehaviour
{
    private Vector3 startPos; // Starting position of the heat indicator
    private bool heating = false; // Is the heat button being pressed
    private bool inThreshold = false; // Is the heat indicator in the threshold
    private float score = 0f; // Initial score

    // Assign in inspector
    public Vector3 maxHeight; // Maximum height of the heat indicator
    public Button heatButton; // Button to control heating
    public float spd; // Speed of heat change
    public CookManager cookManager; // Reference to CookManager
    public TextMeshProUGUI scoreText; // Text to display score
    
    void Start()
    {
        // Initial position to ensure heat doesn't drop infinitely
        startPos = transform.position;

        // Button setup
        var trigger = heatButton.gameObject.AddComponent<EventTrigger>();

        // Button held down
        var down = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        down.callback.AddListener((_) => heating = true);
        trigger.triggers.Add(down);

        // Button released
        var up = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        up.callback.AddListener((_) => heating = false);
        trigger.triggers.Add(up);
    }

    // Update is called once per frame
    void Update()
    {
        if (inThreshold && !cookManager.IsTimeUp())
        {
            // Increase score when in threshold
            score += Time.deltaTime;
        }

        if (heating)
        {
            if (transform.position.y < maxHeight.y)
            {
                // Raise heat if button is held
                transform.position += new Vector3(0, spd * Time.deltaTime, 0);
            }
        }
        else
        {
            if (transform.position.y > startPos.y)
            {
                // Lower heat if button is not held
                transform.position -= new Vector3(0, spd * Time.deltaTime, 0);
                // Ensure heat doesn't go below start position
                if (transform.position.y < startPos.y)
                {
                    transform.position = startPos;
                }
            }
        }

        // Display score
        scoreText.text = "SCORE: " + (Mathf.FloorToInt(score * 10)).ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Threshold"))
        {
            inThreshold = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Threshold"))
        {
            inThreshold = false;
        }
    }
}