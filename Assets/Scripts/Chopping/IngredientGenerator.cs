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

    IEnumerator SpawnIngredients()
    {
        while (true)
        {
            Instantiate(ingredientPrefab, new Vector2(Random.Range(-7f, 7f), -6f), Quaternion.identity, transform);
            yield return new WaitForSeconds(2f);
        }
    }
}
