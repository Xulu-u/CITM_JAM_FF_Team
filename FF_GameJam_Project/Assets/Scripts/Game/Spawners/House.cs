using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House 
{
    public Pathfinding path;
    public List<PathNode> currentPath;

    private GameObject carPrefab;
    private Vector3 position;

    public House(Vector3 position)
    {
        this.position = position;
        this.path = null;
        this.currentPath = null;
    }

    public bool IsPathAvailable(Vector2Int start, Vector2Int end)
    {
        currentPath = path.FindPath(start.x, start.y, end.x, end.y);
        if (currentPath == null)
            return false;
        else
            return true;
    }
}
