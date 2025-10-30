using System.Collections;
using UnityEngine;

public class IngredientGenerator : MonoBehaviour
{
    public GameObject ingredientPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnIngredients());
    }

    // Creates a new Ingredient that shoots up at a random spot from the bottom of the screen
    IEnumerator SpawnIngredients()
    {
        while (true)
        {
            Instantiate(ingredientPrefab, new Vector2(Random.Range(-4f, 4f), -6f), Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }
}
