using System.Collections;
using UnityEngine;
using TMPro;

public class IngredientGenerator : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public GameObject minigameOver;
    [SerializeField] private TextMeshProUGUI finalScoreText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnIngredients());
    }

    // Creates a new Ingredient that shoots up at a random spot from the bottom of the screen
    IEnumerator SpawnIngredients()
    {
        while(GameManager.Instance.GetIngredientCounter()<20) // Runs until 20 non tax folder spawn
        {
            Instantiate(ingredientPrefab, new Vector2(Random.Range(-4f, 4f), -6f), Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
        minigameOver.SetActive(true);
        finalScoreText.text = GameManager.Instance.GetScore();
        
    }
}
