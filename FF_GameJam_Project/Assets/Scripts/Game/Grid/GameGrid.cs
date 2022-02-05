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

    bool debug = false;
    public GameObject debugPrefab;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            debug = !debug;
        }

        if (debug)
        {
            WeDebugginBro();
            debug = false;
        }
    }

    private void WeDebugginBro()
    {   
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = GetWorldPositionFromGrid(gridPos);
                
                switch(entityMap[x, y])
                {
                    case TileFunctionality.TERRAIN:         { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.black); }    break;
                    case TileFunctionality.SPAWN_FACTORY:   { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.magenta); }    break;
                    case TileFunctionality.SPAWN_BASE:      { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.blue); }     break;
                    case TileFunctionality.BRIDGE:          { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.red); }      break;
                    case TileFunctionality.ROAD:            { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.gray); }     break;
                    case TileFunctionality.EMPTY:           { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.green); }  break;
                }
            }
        }
    }

    private void ColorDebugCube(GameObject cube, Color color)
    {
        cube.GetComponentInChildren<MeshRenderer>().material.color = color;
        //clickedCell.GetComponentInChildren<SpriteRenderer>().material.color = Color.black;
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
        FillEntityMapWithTaggedTiles("Terrain", TileFunctionality.TERRAIN);
        FillEntityMapWithTaggedTiles("SpawnFact", TileFunctionality.SPAWN_FACTORY);
        FillEntityMapWithTaggedTiles("SpawnBase", TileFunctionality.SPAWN_BASE);
        FillEntityMapWithTaggedTiles("Bridge", TileFunctionality.BRIDGE);
        FillEntityMapWithTaggedTiles("Empty", TileFunctionality.EMPTY);
    }

    void FillEntityMapWithTaggedTiles(string tag, TileFunctionality tileType)
    {
        GameObject[] tiles;
        tiles = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject tile in tiles)
        {   
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.position - new Vector3(20, 0, 20)/*tile.transform.TransformPoint(tile.transform.localPosition)*/);
            if(pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = tileType;
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
        Debug.Log("Tile Occupied By: " + entityMap[x, y]);

        return (entityMap[x, y] != TileFunctionality.EMPTY);
    }

    public bool IsTileOccupiedByRoad(int x, int y)
    {
        return (entityMap[x, y] == TileFunctionality.ROAD || entityMap[x, y] == TileFunctionality.BRIDGE);
    }
}