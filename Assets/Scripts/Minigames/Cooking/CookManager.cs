using TMPro;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public float seconds;
    public TextMeshProUGUI time;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (seconds > 0)
        {
            // Decrease once per second
            seconds -= Time.deltaTime;

            // Ensure time doesn’t go negative
            if (seconds < 0)
                seconds = 0;
        }

        // Update time display
        time.text = Mathf.CeilToInt(seconds).ToString();
    }

    public bool IsTimeUp()
    {
        return seconds <= 0;
    }

    public int GetTime()
    {
        return Mathf.CeilToInt(seconds);
    }

    public float GetTotalTime() {
        return seconds;
    }
}
