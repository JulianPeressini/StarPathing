using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour 
{
    [SerializeField] private PathGrid grid;

    public Transform target;

    [SerializeField] private float updateTime;
    private float timeToUpdate = 0;

    void Update() 
    {
            Pathfind(gameObject.transform.position, target.position);      
    }

    void Pathfind(Vector3 startPos, Vector3 targetPos) 
    {

        Node startNode = grid.GetNode(startPos);
        Node targetNode = grid.GetNode(targetPos);

        List<Node> openNodes = new List<Node> ();
        List<Node> closedNodes = new List<Node> ();
        openNodes.Add(startNode);

        while (openNodes.Count > 0) 
        {
            if (timeToUpdate > updateTime)
            {
                Node currentNode = openNodes[0];

                for (int i = 1; i < openNodes.Count; i++)
                {
                    if (openNodes[i].travelCost < currentNode.travelCost || openNodes[i].travelCost == currentNode.travelCost && openNodes[i].distanceFromTarget < currentNode.distanceFromTarget)
                    {
                        currentNode = openNodes[i];
                    }
                }

                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

                if (currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach (Node surroundingNode in grid.GetSurroundingNodes(currentNode))
                {
                    if (!surroundingNode.Walkable || closedNodes.Contains(surroundingNode))
                    {
                        continue;
                    }

                    int newTravelCost = currentNode.distanceFromStart + GetDistance(currentNode, surroundingNode);

                    if (newTravelCost < surroundingNode.distanceFromStart || !openNodes.Contains(surroundingNode))
                    {
                        surroundingNode.distanceFromStart = newTravelCost;
                        surroundingNode.distanceFromTarget = GetDistance(surroundingNode, targetNode);
                        surroundingNode.parent = currentNode;

                        if (!openNodes.Contains(surroundingNode))
                        {
                            openNodes.Add(surroundingNode);
                        }

                        grid.openNodes = openNodes;
                        grid.closedNodes = closedNodes;
                    }
                }

                timeToUpdate = 0;
            }
            else
            {
                timeToUpdate += 0.1f;
            }
            
        }
    }

    private int GetDistance(Node start, Node target) 
    {
        int distanceX = Mathf.Abs(start.gridX - target.gridX);
        int distanceY = Mathf.Abs(start.gridY - target.gridY);

        if (distanceX > distanceY) 
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        } 
        else 
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }

    private void RetracePath(Node start, Node target) 
    {
        List<Node> path = new List<Node> ();
        Node currentNode = target;

        while (currentNode != start) 
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;
    }
}