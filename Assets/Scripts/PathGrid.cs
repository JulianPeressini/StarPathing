using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    [SerializeField] private float nodeRadius;
    private float nodeDiameter;

    private int gridSizeX;
    private int gridSizeY;

    [SerializeField] private Vector2 gridSize;
    [SerializeField] private LayerMask unwalkableLayer;
    private Node[,] grid;

    public List<Node> path;
    public List<Node> openNodes;
    public List<Node> closedNodes;


    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        GenerateGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (grid != null)
        {

            foreach (Node currentNode in grid)
            {
                if (currentNode.Walkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                if (openNodes != null)
                {
                    if (openNodes.Contains(currentNode))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }

                if (closedNodes != null)
                {
                    if (closedNodes.Contains(currentNode))
                    {
                        Gizmos.color = Color.magenta;
                    }
                }

                if (path != null)
                {
                    if (path.Contains(currentNode))
                    {
                        Gizmos.color = Color.yellow;
                    }
                }

                Gizmos.DrawCube(currentNode.WorldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    private void GenerateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 gridBottomLeftEdge = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridBottomLeftEdge + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius - 0.1f, unwalkableLayer));
                grid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node GetNode(Vector3 WorldPos)
    {
        float posX = Mathf.Clamp01((WorldPos.x + gridSize.x / 2) / gridSize.x);
        float posY = Mathf.Clamp01((WorldPos.z + gridSize.y / 2) / gridSize.y);
        int x = Mathf.RoundToInt((gridSizeX - 1) * posX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * posY);
        return grid[x, y];
    }

    public List<Node> GetSurroundingNodes(Node desiredNode)
    {
        List<Node> SurroundingNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    int nodeToCheckX = desiredNode.gridX + x;
                    int nodeToCheckY = desiredNode.gridY + y;

                    if (nodeToCheckX >= 0 && nodeToCheckX < gridSizeX && nodeToCheckY >= 0 && nodeToCheckY < gridSizeY)
                    {
                        SurroundingNodes.Add(grid[nodeToCheckX, nodeToCheckY]);
                    }
                }
            }
        }

        return SurroundingNodes;
    }
}
