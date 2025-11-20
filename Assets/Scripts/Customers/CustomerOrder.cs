using UnityEngine;
using System.Collections.Generic;

public class CustomerOrder : MonoBehaviour
{
    private static readonly string[] INGREDIENTS = {"carrots", "bok choy", "broccoli", "peas", "onion", "cabbage", "mushroom" };

    public List<string> Ingredients {get; private set;}
    public CustomerController controller;

    public void Init(CustomerController controller) {
        this.controller = controller;
    }

    public void GenerateOrder() {
        Ingredients = new List<string>();

        int count = Random.Range(2, 6); // pick 2-5 ingredients
        while (Ingredients.Count < count) {
            string ingredient = INGREDIENTS[Random.Range(0, INGREDIENTS.Length)];
            if (!Ingredients.Contains(ingredient)) {
                Ingredients.Add(ingredient);
            }
        }
    }

    public void SetIngredients(System.Collections.Generic.List<string> ingredients) {
        Ingredients = new System.Collections.Generic.List<string>(ingredients);
    }
}

