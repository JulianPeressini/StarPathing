using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;

    private bool walkable;
    private Vector3 worldPos;

    public int distanceFromStart; //G
    public int distanceFromTarget; //H
    public int travelCost; //F
    public int gridX;
    public int gridY;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public Vector3 WorldPos { get { return worldPos;}}
    public bool Walkable { get { return walkable; } }
    public int TravelCost { get { return distanceFromStart + distanceFromTarget; } }
}
