using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HouseType
{
    COAL,
    WOOL
}

public class House : MonoBehaviour
{
    public Pathfinding      path;
    public List<PathNode>   currentPath;

    private GameObject      carPrefab;
    private Vector3         position;
    private HouseType       type;

    public House(Vector3 position, HouseType type)
    {
        this.position       = position;
        this.type           = type;
        this.path           = null;
        this.currentPath    = null;
    }

    public bool IsPathAvailable(Vector2Int start, Vector2Int end)
    {
        currentPath = path.FindPath(start.x, start.y, end.x, end.y);
        return (currentPath == null);
    }
}
