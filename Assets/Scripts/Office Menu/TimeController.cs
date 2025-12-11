using UnityEngine;
using TMPro;
using System.Collections;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    private int minutes = 0;
    private int hours = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        if(minutes < 59)
        {
            minutes++;
        }
        else
        {
            minutes = 0;
            hours++;
            if (hours > 23)
            {
                hours = 0;
            }
        }
        timeText.text = $"{hours:00}:{minutes:00}";
    }
}
