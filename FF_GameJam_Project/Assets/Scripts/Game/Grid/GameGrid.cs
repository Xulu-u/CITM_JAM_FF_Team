using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NON_WALKABLE,
    WALKABLE,
    START_COAL,
    END_COAL,
    START_WOOL,
    END_WOOL
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

public struct FactoryArea
{
    public List<Vector2Int>    tiles;
    public Vector2Int          entrance;
    public bool                isHorizontal;

    public void FillTiles(Vector2Int pivotPos, bool horizontal)
    {
        tiles           = new List<Vector2Int>();
        isHorizontal    = horizontal;

        if (horizontal)
        {
            for (int y = 0; y < 2; ++y)
            {
                for (int x = 0; x < 3; ++x)
                {
                    tiles.Add(new Vector2Int(pivotPos.x + x, pivotPos.y + y));
                    if (x == 1 && y == 1) { entrance = tiles[tiles.Count - 1]; }
                }
            }
        }
        else
        {
            for (int y = 0; y < 3; ++y)
            {
                for (int x = 0; x < 2; ++x)
                {
                    tiles.Add(new Vector2Int(pivotPos.x + x, pivotPos.y + y));
                    if (x == 1 && y == 0) { entrance = tiles[tiles.Count - 1]; }
                }
            }
        }
    }
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
    private GameObject[,] gameGrid;                         //Grid that allow us to click on different gridCells 
    private TileType[,] walkabilityMap;                     //Functionality that will help us to use Pathfinding.
    private TileFunctionality[,] entityMap;                 //Read gameobject tags and adds functionality to the map so later we can use spawn etc..
    private Tile[,] tileMap;                                //

    [HideInInspector] public GridPathfinding pathGrid;
    [HideInInspector] public Pathfinding path;

    [HideInInspector] public List<FactoryArea> factoryAreas  = new List<FactoryArea>();

    bool debugEntityMap         = false;
    bool debugWalkabilityMap    = false;
    public GameObject debugPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        originPosition  = originGrid.transform.position;
        walkabilityMap  = new TileType[height, width];
        entityMap       = new TileFunctionality[height, width];
        tileMap         = new Tile[height, width];

        path = new Pathfinding(width, height);
        pathGrid = path.GetGrid();
        //pathGrid = new GridPathfinding(height, width, 20f, originPosition);
        //pathGrid.gameGrid = this;

        CreateGrid();
        CreateEntityMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { debugEntityMap = !debugEntityMap; }
        if (Input.GetKeyDown(KeyCode.O)) { debugWalkabilityMap = !debugWalkabilityMap; }

        if (debugEntityMap)
        {
            DebugEntityMap();
            debugEntityMap = false;
        }
        if (debugWalkabilityMap)
        {
            DebugWalkabilityMap();
            debugWalkabilityMap = false;
        }
    }

    private void DebugEntityMap()
    {   
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Vector2Int gridPos  = new Vector2Int(x, y);
                Vector3 worldPos    = GetWorldPositionFromGrid(gridPos);

                switch(entityMap[x, y])
                {
                   case TileFunctionality.TERRAIN:         { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.black); }    break;
                   case TileFunctionality.SPAWN_FACTORY:   { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.magenta); }  break;
                   case TileFunctionality.SPAWN_BASE:      { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.blue); }     break;
                   case TileFunctionality.BRIDGE:          { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.red); }      break;
                   case TileFunctionality.ROAD:            { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.gray); }     break;
                   case TileFunctionality.EMPTY:           { ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.green); }    break;
                }
            }
        }
    }

    private void DebugWalkabilityMap()
    {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Vector2Int gridPos  = new Vector2Int(x, y);
                Vector3 worldPos    = GetWorldPositionFromGrid(gridPos);
                
                switch (walkabilityMap[x, y])
                {
                    case TileType.NON_WALKABLE: {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.black);}      break;
                    case TileType.WALKABLE:     {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.green);}      break;
                    case TileType.START_COAL:   {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.magenta);}    break;
                    case TileType.START_WOOL:   {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.cyan);}       break;
                    case TileType.END_COAL:     {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.red);}        break;
                    case TileType.END_WOOL:     {ColorDebugCube(Instantiate(debugPrefab, new Vector3(worldPos.x + 10, 0.5f, worldPos.z + 10), Quaternion.identity), Color.blue);}       break;
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

        GameObject[] factPivots = GameObject.FindGameObjectsWithTag("FactoryPivot");
        foreach (GameObject pivot in factPivots)
        {
            Vector2Int pos = GetGridPositionFromWorld(pivot.transform.position);
            if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                FactoryArea fArea = new FactoryArea();
                fArea.FillTiles(pos, (pivot.name == "HorizontalFactory"));
                factoryAreas.Add(fArea);
            }
        }
    }

    void FillEntityMapWithTaggedTiles(string tag, TileFunctionality tileType)
    {
        GameObject[] tiles;
        tiles = GameObject.FindGameObjectsWithTag(tag);

        Vector3 offset = Vector3.zero;

        switch(tileType)
        {
            case TileFunctionality.TERRAIN:         { offset = new Vector3(10, 0, 10); } break;
            case TileFunctionality.SPAWN_FACTORY:   { offset = new Vector3(10, 0, -1); } break;
            case TileFunctionality.SPAWN_BASE:      { offset = new Vector3(10, 0, 10); } break;
            case TileFunctionality.BRIDGE:          { offset = new Vector3(10, 0, 10); } break;
            case TileFunctionality.ROAD:            { offset = new Vector3(10, 0, 10); } break;
            case TileFunctionality.EMPTY:           { offset = new Vector3(10, 0, 10); } break;
        }

        foreach (GameObject tile in tiles)
        {   
            Vector2Int pos = GetGridPositionFromWorld(tile.transform.position - offset);
            if(pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
            {
                entityMap[pos.x, pos.y] = tileType;

                if (tileType == TileFunctionality.BRIDGE)   { walkabilityMap[pos.x, pos.y] = TileType.WALKABLE; }
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

    public void SetTileWalkable(int x, int y, TileType type)
    {
        walkabilityMap[x, y] = type;
    }

    public bool TileIsWalkable(int x, int y)
    {
        return (walkabilityMap[x, y] == TileType.NON_WALKABLE);
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
        //Debug.Log("Tile Occupied By: " + entityMap[x, y]);

        return (entityMap[x, y] != TileFunctionality.EMPTY);
    }

    public bool IsTileOccupiedByRoad(int x, int y)
    {
        return (entityMap[x, y] == TileFunctionality.ROAD || entityMap[x, y] == TileFunctionality.BRIDGE);
    }

    public bool IsTileOccupiedByBridge(int x, int y)
    {
        return (entityMap[x, y] == TileFunctionality.BRIDGE);
    }

    public bool TileExists(int x, int y)
    {
        return (tileMap[x, y] != null);
    }

    public Vector2Int GetRandomEmptyTile()
    {
        List<Vector2Int> emptyTiles = new List<Vector2Int>();

        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (entityMap[x, y] == TileFunctionality.EMPTY)
                {
                    emptyTiles.Add(new Vector2Int(x, y));
                }
            }
        }

        return emptyTiles[Random.Range(0, emptyTiles.Count - 1)];
    }

    public Vector3Int GetRandomFactoryTile()
    {
        FactoryArea fArea   = factoryAreas[Random.Range(0, factoryAreas.Count - 1)];
        Vector3Int rng      = new Vector3Int(fArea.entrance.x, fArea.entrance.y, (fArea.isHorizontal) ? 0 : 1);     // Sometimes you have to do some questionable stuff.
        factoryAreas.Remove(fArea);

        walkabilityMap[rng.x, rng.y] = TileType.END_COAL;

        return rng;
    }
}