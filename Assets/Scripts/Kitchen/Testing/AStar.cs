using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    private static Vector2 gridSize;
    private class Node
    {
        public Vector2Int position;
        public Node parent; // Node which we came from
        public float gCost; // Cost from start to current node
        public float hCost; // Heuristic cost to end node
        public float fCost => gCost + hCost; // Total cost
        public bool isObstacle; // Is this node an obstacle

        public Node(Vector2Int position, Node parent, float gCost, float hCost, bool isObstacle)
        {
            this.position = position;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
            this.isObstacle = isObstacle;
        }
    }

    // Main A* pathfinding method
    public static List<Vector2> FindPath(Vector2 start, Vector2 target, Vector2 gridCellSize, System.Func<Vector2, bool> isObstacle)
    {
        // Set grid size for use in other methods
        gridSize = gridCellSize;

        // Get grid coordinates of player's current location and target location (where they clicked)
        Vector2Int startGridPos = GridUtils.WorldToGrid(start);
        Vector2Int targetGridPos = GridUtils.WorldToGrid(target);

        List<Node> openList = new List<Node>(); // Nodes we haven't evaluated yet
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>(); // Nodes we've already evaluated

        Node startNode = new Node(startGridPos, null, 0, CalculateHCost(startGridPos, targetGridPos), false); // Convert start position to a node
        openList.Add(startNode); // Add start node to open list

        while (openList.Count > 0) // While there are nodes to evaluate
        {
            // Get node with lowest fCost, and mark it as the current node
            Node currentNode = GetLowestFCostNode(openList);
            openList.Remove(currentNode);
            closedSet.Add(currentNode.position);

            if (currentNode.position == targetGridPos) // If we've reached the target node, generate the path
            {
                return GeneratePath(currentNode);
            }

            // for each adjacent cell to the current node (up, down, left, right)
            foreach (Vector2Int adjCell in GetAdjacentCells(currentNode.position))
            {
                // Skip if already evaluated or is an obstacle
                if (closedSet.Contains(adjCell) || isObstacle(GridUtils.GridToWorld(adjCell)))
                {
                    continue;
                }
                // Calculate costs and add to open list  
                Node adjNode = new Node(adjCell, currentNode, currentNode.gCost + 1, CalculateHCost(adjCell, targetGridPos), false);
                int index = openList.FindIndex(n => n.position == adjCell); // Check if node is already in open list
                if (index != -1)
                {
                    // If this path to that node is better, update its costs and parent
                    if (adjNode.gCost < openList[index].gCost)
                    {
                        openList[index] = adjNode;
                        openList[index].gCost = adjNode.gCost;
                    }
                }
                else
                {
                    openList.Add(adjNode);
                }
            }
        }
        return null;
    }

    private static Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowestNode = nodes[0];

        foreach (Node node in nodes)
        {
            if (node.fCost <= lowestNode.fCost || (node.fCost == lowestNode.fCost && node.hCost < lowestNode.hCost))
            {
                lowestNode = node;
            }
        }
        return lowestNode;
    }

    private static float CalculateHCost(Vector2Int current, Vector2Int target)
    {
        return Mathf.Abs(current.x - target.x) + Mathf.Abs(current.y - target.y);
    }

    private static List<Vector2Int> GetAdjacentCells(Vector2Int position)
    {
        return new List<Vector2Int>
            {
                position + Vector2Int.up,
                position + Vector2Int.down,
                position + Vector2Int.left,
                position + Vector2Int.right,
            };
    }

    private static List<Vector2> GeneratePath(Node node)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode != null)
        {
            path.Add(GridUtils.GridToWorld(currentNode.position));
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}