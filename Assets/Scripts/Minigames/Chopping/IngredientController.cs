using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))] 
public class IngredientController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 12.5f; 
    public IngredientSprites[] ingredientSprites; // Array of ingredient sprite objects to randomly choose from (see IngredientSprites.cs)
    public IngredientSprites selectedSprite; // The selected sprite object for this instance 

    [HideInInspector] public int vegetableCounter = 0;
    [HideInInspector] public SpriteRenderer sr;    
    [HideInInspector] public bool isCut, isLeft = false; // Checks to see if the instance is cut and if it's the left half

    private HashSet<string> allowedIngredients;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        allowedIngredients = BuildAllowedIngredients();

        if (isCut) // If the ingredient has been cut
        {
            if (isLeft) // If the cut ingredient is the left half
            {
                rb.linearVelocity = new Vector2(-4f, -4f); // Make the instance fall to the left
            }
            else
            {
                rb.linearVelocity = new Vector2(4f, -4f); // Make the instance fall to the right
            }
        }
        else
        {
            SelectSpriteForOrder();
            sr.sprite = selectedSprite.full; // Set the sprite to the full ingredient sprite

            rb.linearVelocity = new Vector2(Random.Range(-4f, 4f), speed); // Make the instance shoot up at a random angle
        }
    }
    
    // Destroy the instance when it goes off-screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void SelectSpriteForOrder()
    {
        if (ingredientSprites == null || ingredientSprites.Length == 0)
            return;

        bool spawnTax = Random.Range(0, 10) == 0; // 10% chance to spawn a tax folder instead of an ingredient
        int taxIndex = ingredientSprites.Length - 1;

        if (spawnTax)
        {
            selectedSprite = ingredientSprites[taxIndex];
            return;
        }

        var candidates = new List<IngredientSprites>();
        // If allowedIngredients is empty, fall back to all non-tax sprites
        bool restrict = allowedIngredients != null && allowedIngredients.Count > 0;

        for (int i = 0; i < ingredientSprites.Length - 1; i++)
        {
            var ing = ingredientSprites[i];
            var name = ing != null ? ing.ingredientName : null;
            if (!restrict || (!string.IsNullOrEmpty(name) && allowedIngredients.Contains(name.ToLowerInvariant())))
                candidates.Add(ing);
        }

        // If none matched, fall back to any non-tax ingredient
        if (candidates.Count == 0)
        {
            for (int i = 0; i < ingredientSprites.Length - 1; i++)
                candidates.Add(ingredientSprites[i]);
        }

        selectedSprite = candidates[Random.Range(0, candidates.Count)];
        ChoppingGameManager.Instance.AddIngredient();
    }

    private HashSet<string> BuildAllowedIngredients()
    {
        // Prefer the explicitly selected minigame order; fallback to first available
        OrderSystem.OrderData order = OrderSystem.ActiveMinigameOrder;
        if (order == null && OrderSystem.ActiveOrders != null && OrderSystem.ActiveOrders.Count > 0)
            order = OrderSystem.ActiveOrders[0];

        if (order?.Ingredients == null || order.Ingredients.Count == 0)
            return null;

        var set = new HashSet<string>();
        foreach (var ing in order.Ingredients)
        {
            if (!string.IsNullOrWhiteSpace(ing))
                set.Add(ing.Trim().ToLowerInvariant());
        }
        return set;
    }
}
