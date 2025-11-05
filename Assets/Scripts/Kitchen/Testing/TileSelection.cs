using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelection : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap counterTilemap;
    [SerializeField] private float offset = 0.5f;
    [SerializeField] private Vector2 gridSize = new Vector2(1f, 1f);
    private Vector2Int highlightedTilePosition = Vector2Int.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(
            Mathf.FloorToInt(mouseWorldPos.x / gridSize.x) * Mathf.RoundToInt(gridSize.x),
            Mathf.FloorToInt(mouseWorldPos.y / gridSize.y) * Mathf.RoundToInt(gridSize.y)
        );

        // Check if the mouse is over a counter tile
        bool isObstacleTile = false;
        if (counterTilemap != null)
        {
            Vector3Int cellPos = counterTilemap.WorldToCell(mouseWorldPos);
            if (counterTilemap.HasTile(cellPos) && counterTilemap.GetTile(cellPos) != null)
            {
                // Mouse is over a counter tile
                isObstacleTile = true;
            }
        }

        if (!isObstacleTile)
        {
            highlightedTilePosition = gridPos;
            Vector2 worldPos = GridUtils.GridToWorld(gridPos) + new Vector2(offset, offset);
            transform.position = worldPos;
        }
    }

    public Vector2Int HighlightedTilePosition
    {
        get { return highlightedTilePosition; }
    }

    public bool IsHighlightedTileClicked(Vector2 clickedPosition)
    {
        Vector2Int gridPos = GridUtils.WorldToGrid(clickedPosition);
        return gridPos == highlightedTilePosition;
    }

    public Vector2 GetHighlightedTilePosition()
    {
        return GridUtils.GridToWorld(highlightedTilePosition);
    }

    public bool IsTileObstacle(Vector2Int position)
    {
        Vector3 worldPosition = GridUtils.GridToWorld(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        // Check if there's a collider hit
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

}
