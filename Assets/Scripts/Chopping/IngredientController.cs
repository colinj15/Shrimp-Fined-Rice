using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 12.5f; 
    public IngredientSprites[] ingredientSprites; // Array of ingredient sprite objects to randomly choose from (see IngredientSprites.cs)
    public IngredientSprites selectedSprite; // The selected sprite object for this instance 

    [HideInInspector]
    public SpriteRenderer sr;
    
    [HideInInspector]
    public bool isCut, isLeft = false; // Checks to see if an ingredient has already been cut and if it's the left half or not

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

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
            selectedSprite = ingredientSprites[Random.Range(0, ingredientSprites.Length)]; // Randomly select an ingredient sprite object
            sr.sprite = selectedSprite.full; // Set the sprite to the full ingredient sprite
            
            rb.linearVelocity = new Vector2(Random.Range(-4f, 4f), speed); // Make the instance shoot up at a random angle
        }
    }
}
