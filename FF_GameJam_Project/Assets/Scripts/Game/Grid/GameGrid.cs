using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType
{
    NON_WALKABLE,
    WALKABLE
}
public enum TileFunctionality
{
    TERRAIN,
    SPAWN_FACTORY,
    SPAWN_BASE,
    EMPTY,
    BRIDGE,
    ROAD
}

public class GameGrid : MonoBehaviour
{
    public int height;
    public int width;
    private float gridSpaceSize = 20f;
    private Vector3 originPosition;

    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private GameObject originGrid;

    //different grids
    private GameObject[,] gameGrid;                 //Grid that allow us to click on different gridCells 
    private TileType[,] walkabilityMap;             //Functionality that will help us to use Pathfinding.
    private TileFunctionality[,] entityMap;        //Read gameobject tags and adds functionality to the map so later we can use spawn etc..

    private Tile[,] tileMap;

    // Start is called before the first frame update
    void Start()
    {
        originPosition  = originGrid.transform.position;
        walkabilityMap  = new TileType[height, width];
        entityMap       = new TileFunctionality[height, width];
        tileMap         = new Tile[height, width];
        CreateGrid();
        CreateEntityMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateGrid()
    {
        gameGrid = new GameObject[height, width];
        if(gridCellPrefab == null)
        {
            Debug.Log("ERROR: Grid cell prefab is not assigned");
            return;
        }

        for(int y= 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x )
            {
                
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 1f,  y * gridSpaceSize)+ originPosition, Quaternion.Euler(90,0,0));
                gameGrid[x, y].GetComponent<gridCell>().SetPosition(x, y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grid Space (X: " + x.ToString() + "Y: " + y.ToString() + ")";

                walkabilityMap[x, y] = TileType.NON_WALKABLE;
            }
        }
    }

    private void CreateEntityMap()
    {
        GameObject[] terrains;
        terrains = GameObject.FindGameObjectsWithTag("Terrain");

        foreach( GameObject tile in terrains)
        {
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.TransformPoint(tile.transform.localPosition));
            if(pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = TileFunctionality.TERRAIN;
            }
        }

        GameObject[] spawnFactory;
        spawnFactory = GameObject.FindGameObjectsWithTag("SpawnFact");

        foreach (GameObject tile in spawnFactory)
        {
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.TransformPoint(tile.transform.localPosition));
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = TileFunctionality.SPAWN_FACTORY;
            }
        }

        GameObject[] spawnBase;
        spawnBase = GameObject.FindGameObjectsWithTag("SpawnBase");

        foreach (GameObject tile in spawnBase)
        {
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.TransformPoint(tile.transform.localPosition));
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = TileFunctionality.SPAWN_BASE;
            }
        }

        GameObject[] editable;
        editable = GameObject.FindGameObjectsWithTag("Empty");

        foreach (GameObject tile in editable)
        {
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.TransformPoint(tile.transform.localPosition));
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = TileFunctionality.EMPTY;
            }
        }

        GameObject[] bridges;
        bridges = GameObject.FindGameObjectsWithTag("Bridge");

        foreach (GameObject tile in bridges)
        {
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.TransformPoint(tile.transform.localPosition));
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = TileFunctionality.BRIDGE;
            }
        }
    }

    public Vector2Int GetGridPositionFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / gridSpaceSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPositionFromGrid(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float y = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, y) + originPosition;
    }

    public void SetTileWalkable(int x, int y)
    {
        walkabilityMap[x, y] = TileType.WALKABLE;
    }

    public void SetEntity(int x, int y, TileFunctionality entityType)
    {
        entityMap[x, y] = entityType;
    }

    public Tile GetTile(int x, int y)
    {
        return tileMap[x, y];
    }

    public void SetTile(int x, int y, Tile newTile)
    {
        tileMap[x, y] = newTile;
    }

    public bool IsTileOccupied(int x, int y)
    {
        //return gameGrid[x, y].GetComponent<gridCell>().isOcupied;
        return entityMap[x, y] != TileFunctionality.EMPTY;
    }

    public bool IsTileOccupiedByRoad(int x, int y)
    {
        return (entityMap[x, y] == TileFunctionality.ROAD || entityMap[x, y] == TileFunctionality.BRIDGE);
    }
}