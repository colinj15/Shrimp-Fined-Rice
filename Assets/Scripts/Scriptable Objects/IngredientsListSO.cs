using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "IngredientList", menuName = "Ingredients/IngredientList")]
public class IngredientListSO : ScriptableObject {
    public List<Ingredient> ingredients = new List<Ingredient>();
}
