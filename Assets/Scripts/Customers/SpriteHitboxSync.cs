using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SpriteHitboxSync : MonoBehaviour
{
    private SpriteRenderer sr;
    private PolygonCollider2D poly;
    private readonly List<Vector2> buffer = new List<Vector2>(256);

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        poly = GetComponent<PolygonCollider2D>();
    }

    public void ChangeSprite(Sprite newSprite)
    {
        sr.sprite = newSprite;
        UpdateColliderToSprite();
    }

    private void UpdateColliderToSprite()
    {
        var sprite = sr.sprite;
        if (sprite == null)
        {
            poly.pathCount = 0;
            return;
        }

        int shapeCount = sprite.GetPhysicsShapeCount();

        if (shapeCount > 0)
        {
            // Copy custom physics shapes
            poly.pathCount = shapeCount;
            for (int i = 0; i < shapeCount; i++)
            {
                buffer.Clear();
                sprite.GetPhysicsShape(i, buffer);
                poly.SetPath(i, buffer);
            }
        }
        else
        {
            // Fallback: simple rectangle around sprite bounds
            var b = sprite.bounds;
            Vector2[] rect = new Vector2[]
            {
                new Vector2(b.min.x, b.min.y),
                new Vector2(b.min.x, b.max.y),
                new Vector2(b.max.x, b.max.y),
                new Vector2(b.max.x, b.min.y)
            };
            poly.pathCount = 1;
            poly.SetPath(0, rect);
        }

        poly.offset = Vector2.zero;
    }
}
