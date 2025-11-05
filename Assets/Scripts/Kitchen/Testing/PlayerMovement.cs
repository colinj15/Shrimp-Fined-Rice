using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector2 targetPosition;
    private bool isMoving = false;
    private MovementDirection currentDirection = MovementDirection.Down;

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
            Vector2Int clickedTile = GridUtils.WorldToGrid(targetPosition);

            if (!counterTiles.IsTileCounter(clickedTile))
            {
                if (targetPosition != Vector2.zero)
                {
                    FindPathToTargetPosition();
                }
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

        isMoving = false;
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
