using UnityEngine;
using UnityEngine.InputSystem;

public class CabinetController : MonoBehaviour
{
    public enum Veggie
    {
        Carrot,
        Broccoli,
        BokChoy,
        Mushroom,
        Cabbage,
        Onion,
        Peas
    }

    public Veggie containedVeggie;
    public GameObject veggiePrefab;
    private Camera cam;
    private Collider2D selfCollider;

    void Start()
    {
        cam = Camera.main;
        selfCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider == selfCollider)
            {
                Debug.Log("Clicked: " + hit.collider.gameObject.name);
                SpawnVeggie();
            }
        }
    }
    
    void SpawnVeggie()
    {
        // Instantiate the veggie prefab at the cabinet's position
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0);
        var veggie = Instantiate(veggiePrefab, spawnPosition, Quaternion.identity);

        var vc = veggie.GetComponent<VeggieController>();
        vc.veggieType = containedVeggie;
    }
}
