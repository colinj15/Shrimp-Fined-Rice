using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public enum MovementDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 gridSize = new Vector2(1f, 1f);
    [SerializeField] private CounterTiles counterTiles;
    [SerializeField] private TileSelection tileSelection;
    [SerializeField] private Tilemap washingTilemap, cookingTilemap, choppingTilemap, fryingTilemap, officeTilemap;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private MovementDirection currentDirection = MovementDirection.Down;
    private bool changeScene = false;
    private string sceneToLoad = "";

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            targetPosition = tileSelection.GetHighlightedTilePosition();
            Vector2Int clickedTileCell = GridUtils.WorldToGrid(targetPosition);

            if (!counterTiles.IsTileCounter(clickedTileCell))
            {
                if (targetPosition != Vector2.zero)
                {
                    FindPathToTargetPosition();
                }
            }
            TileBase washingTile = null;
            TileBase cookingTile = null;
            TileBase choppingTile = null;
            TileBase fryingTile = null;
            TileBase officeTile = null;

            if (washingTilemap != null)
                washingTile = washingTilemap.GetTile((Vector3Int)clickedTileCell);

            if (cookingTilemap != null)
                cookingTile = cookingTilemap.GetTile((Vector3Int)clickedTileCell);

            if (choppingTilemap != null)
                choppingTile = choppingTilemap.GetTile((Vector3Int)clickedTileCell);

            if (fryingTilemap != null)
                fryingTile = fryingTilemap.GetTile((Vector3Int)clickedTileCell);
            if (officeTilemap != null)
                officeTile = officeTilemap.GetTile((Vector3Int)clickedTileCell);

            // Then your logic
            if (washingTile != null)
            {
                changeScene = true;
                sceneToLoad = "Washing";
            }
            else if (cookingTile != null)
            {
                changeScene = true;
                sceneToLoad = "Cooking";
            }
            else if (choppingTile != null)
            {
                changeScene = true;
                sceneToLoad = "Chopping";
            }
            else if (fryingTile != null)
            {
                changeScene = true;
                sceneToLoad = "Frying";
            }
            else if (officeTile != null)
            {
                changeScene = true;
                sceneToLoad = "Computer Menu";
            }
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void FindPathToTargetPosition()
    {
        Vector2 startPosition = GridUtils.GridToWorld(GridUtils.WorldToGrid(transform.position));
        List<Vector2> path = AStar.FindPath(startPosition, targetPosition, gridSize, counterTiles.IsTileCounter);

        if (path != null && path.Count > 0)
        {
            StartCoroutine(MoveAlongPath(path));
        }
    }

    private IEnumerator MoveAlongPath(List<Vector2> path)
    {
        isMoving = true;
        int currentWayPointIndex = 0;

        while (currentWayPointIndex < path.Count)
        {
            targetPosition = path[currentWayPointIndex] + gridSize / 2;
            while ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            currentWayPointIndex++;
        }
        Debug.Log("Reached Target Position");
        isMoving = false;
        if (changeScene && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                currentDirection = MovementDirection.Right;
            else
                currentDirection = MovementDirection.Left;
        }
        else
        {
            if (direction.y > 0)
                currentDirection = MovementDirection.Up;
            else
                currentDirection = MovementDirection.Down;
        }
    }
}
