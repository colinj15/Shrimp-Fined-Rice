using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D hitbox;
    public SpriteHitboxSync spriteHitboxSync;
    private string customerName;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setName(string name)
    {
        customerName = name;
    }

    public string getName()
    {
        return customerName;
    }

    public void changeSprite(Sprite newSprite)
    {
        spriteHitboxSync.ChangeSprite(newSprite);
    }
}
