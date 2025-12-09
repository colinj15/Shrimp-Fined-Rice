using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    private int score = 0;
    private List<VeggieController> veggies = new List<VeggieController>();
    public TextMeshProUGUI scoreText;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var veggie = collision.GetComponent<VeggieController>();
        veggies.Add(veggie);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var veggie = collision.GetComponent<VeggieController>();
        veggies.Remove(veggie);
    }

    // Update is called once per frame
    void Update()
    {
        score = 0;
        for (int i = 0; i < veggies.Count; i++)
        {
            var veggie = veggies[i];
            if (veggie == null)
            {
                veggies.RemoveAt(i);
                i--;
                continue;
            }

            if (veggie.dirtyness <= 0f)
            {
                score += 1;
                /*Destroy(veggie.gameObject);
                veggies.RemoveAt(i);
                i--;*/
            }
        }
        scoreText.text = "Score: " + score;
    }

    public int GetWeightedScore() {
        // Example weighted score: each clean veggie counts as 1, partial cleanliness contributes proportionally
        if (veggies == null || veggies.Count == 0) return 0;

        float total = 0f;
        int counted = 0;

        for (int i = 0; i < veggies.Count; i++) {
            var veggie = veggies[i];
            if (veggie == null) continue;

            // Assume dirtyness ranges 0 (clean) to 1 (fully dirty); weight inversely
            float cleanliness = Mathf.Clamp01(1f - veggie.dirtyness);
            total += cleanliness;
            counted++;
        }

        // Return rounded weighted score
        return Mathf.RoundToInt(total);
    }

}
