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

    public GameObject      carPrefab;
    private Vector3         position;
    public Vector2Int       startPoint;
    public Vector2Int       endPoint;
    public GameGrid         grid;
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
        return (currentPath != null);
    }

    private void Start()
    {
     

    }

    private void Update()
    {
        //Debuging Path:
        if(IsPathAvailable(startPoint, endPoint))
        {
            for(int i = 0; i < currentPath.Count - 1; ++i)
            {
                Vector2Int pos = currentPath[i].getNodePosition();
                Instantiate(carPrefab, grid.GetWorldPositionFromGrid(pos), Quaternion.identity);
            }
        }
    }
}
