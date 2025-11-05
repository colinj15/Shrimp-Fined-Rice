using UnityEngine;
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
            if (Random.Range(0, 10) == 0) // 10% chance to spawn a tax folder instead of an ingredient
            {
                selectedSprite = ingredientSprites[ingredientSprites.Length - 1];
            }
            else
            {
                selectedSprite = ingredientSprites[Random.Range(0, ingredientSprites.Length - 1)]; // Randomly select an ingredient sprite object
                GameManager.Instance.AddIngredient();
            }
            sr.sprite = selectedSprite.full; // Set the sprite to the full ingredient sprite

            rb.linearVelocity = new Vector2(Random.Range(-4f, 4f), speed); // Make the instance shoot up at a random angle
        }
    }
    
    // Destroy the instance when it goes off-screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
