using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathfinding
{
    private int height;
    private int width;
    private float gridSize = 20f;
    private Vector3 originPosition;
    public PathNode[,] pathGrid;

    public GridPathfinding(int height, int width, float gridSize, Vector3 originPosition)
    {
        this.height = height;
        this.width = width;
        this.gridSize = gridSize;
        this.originPosition = originPosition;

        pathGrid = new PathNode[height, width];

        for(int y = 0; y < width; ++y)
        {
            for(int x = 0; x < height; ++x)
            {
                PathNode node = new PathNode(this, x, y);
                pathGrid[x, y] = node;
            }
        }
    }


    public PathNode GetGridObject(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < width && y < height) ? pathGrid[x, y] : default;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}

