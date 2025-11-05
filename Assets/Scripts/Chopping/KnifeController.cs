using UnityEngine;

public class KnifeController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(mousePosition); // Move the knife to the mouse position
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject cutObject = collision.gameObject;
        // If the object being cut is an ingredient and hasn't already been cut
        if (cutObject.CompareTag("Ingredient") && !cutObject.GetComponent<IngredientController>().isCut)
        {
            CutIngredient(cutObject);
            Destroy(cutObject);

        }
    }
    
    public void CutIngredient(GameObject ingredient)
    {
        // Create two new ingredient halves at the position of the original ingredient and controllers for each half
        GameObject cutIngredientLeft = Instantiate(ingredient, ingredient.transform.position, Quaternion.identity, transform);
        GameObject cutIngredientRight = Instantiate(ingredient, ingredient.transform.position, Quaternion.identity, transform);
        IngredientController leftController = cutIngredientLeft.GetComponent<IngredientController>();
        IngredientController rightController = cutIngredientRight.GetComponent<IngredientController>();

        // Modify the controllers to reflect that the ingredients have been cut and set their sprites to the appropriate halves
        leftController.isCut = true;
        leftController.isLeft = true;
        leftController.sr.sprite = ingredient.GetComponent<IngredientController>().selectedSprite.left;

        rightController.isCut = true;
        rightController.sr.sprite = ingredient.GetComponent<IngredientController>().selectedSprite.right;
        
        // If the cut ingredient is a tax folder
        if(leftController.selectedSprite.full.name == "Tax Folder_0")
        {
            GameManager.Instance.RemoveScore(); // Decrease score
        }
        else
        {
            GameManager.Instance.AddScore(); // Increase score
        }
    }
}
