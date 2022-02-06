using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPathfinding grid;
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public bool isWalkable;
    public PathNode(GridPathfinding grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        isWalkable = false;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public Vector2Int getNodePosition()
    {
        return new Vector2Int(x, y);
    }
}
