using UnityEngine;

[System.Serializable]
// Class that holds the full ingredient sprite and its two halves (called in IngredientController.cs)
public class IngredientSprites
{
    [Tooltip("Name that should match the order ingredient string (case-insensitive).")]
    public string ingredientName;
    public Sprite full;
    public Sprite left;
    public Sprite right;
}
