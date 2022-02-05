using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<GameObject> statePrefabs    = new List<GameObject>();
    
    [HideInInspector] public GameGrid grid  = null;
    [HideInInspector] public gridCell cell  = null;
    
    private GameObject currentState         = null;    
    

    enum TILE_STATE
    {
        UP = 1,
        LEFT,
        UP_LEFT,
        DOWN,
        V_STRAIGHT,
        DOWN_LEFT,
        T_LEFT,
        RIGHT,
        UP_RIGHT,
        H_STRAIGHT,
        T_UP,
        DOWN_RIGHT,
        T_RIGHT,
        T_DOWN,
        CROSS
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplyNewState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnDestroy()
    // {
    //     Vector2Int cellPos  = cell.GetPosition();
        
    //     Vector2Int up       = new Vector2Int(cellPos.x - 1, cellPos.y);
    //     Vector2Int left     = new Vector2Int(cellPos.x, cellPos.y - 1);
    //     Vector2Int down     = new Vector2Int(cellPos.x + 1, cellPos.y);
    //     Vector2Int right    = new Vector2Int(cellPos.x, cellPos.y + 1);

    //     if (grid.IsTileOccupiedByRoad(up.x, up.y))          { grid.GetTile(up.x, up.y).ReactToState(); }
    //     if (grid.IsTileOccupiedByRoad(left.x, left.y))      { grid.GetTile(left.x, left.y).ReactToState(); }
    //     if (grid.IsTileOccupiedByRoad(down.x, down.y))      { grid.GetTile(down.x, down.y).ReactToState(); }
    //     if (grid.IsTileOccupiedByRoad(right.x, right.y))    { grid.GetTile(right.x, right.y).ReactToState(); }
    // }

    void ApplyNewState()
    {
        // Check Neighbours and get id
        Vector2Int cellPos  = cell.GetPosition();
        int id              = 0;
        
        Vector2Int up       = new Vector2Int(cellPos.x - 1, cellPos.y);
        Vector2Int left     = new Vector2Int(cellPos.x, cellPos.y - 1);
        Vector2Int down     = new Vector2Int(cellPos.x + 1, cellPos.y);
        Vector2Int right    = new Vector2Int(cellPos.x, cellPos.y + 1);

        if (grid.IsTileOccupiedByRoad(up.x, up.y))                                    // UP
        { 
            id += 1; 
            grid.GetTile(up.x, up.y).ReactToState();
        }
        if (grid.IsTileOccupiedByRoad(left.x, left.y))                                    // LEFT
        { 
            id += 2;
            grid.GetTile(left.x, left.y).ReactToState();
        }
        if (grid.IsTileOccupiedByRoad(down.x, down.y))                                    // DOWN
        { 
            id += 4;
            grid.GetTile(down.x, down.y).ReactToState();
        }
        if (grid.IsTileOccupiedByRoad(right.x, right.y))                                    // RIGHT
        { 
            id += 8;
            grid.GetTile(right.x, right.y).ReactToState();
        }
        
        if (id == 0)
        {
            // There needs to be an adjacent road or have the drag control.
            id = 1;
            //return;
        }

        Destroy(currentState);
        currentState = Instantiate(statePrefabs[id], transform.position, Quaternion.Euler(statePrefabs[id].transform.localEulerAngles), transform);
    }

    void ReactToState()
    {
        Vector2Int cellPos  = cell.GetPosition();
        int id              = 0;

        if (grid.IsTileOccupiedByRoad(cellPos.x - 1, cellPos.y)) { id += 1;}                           // UP
        if (grid.IsTileOccupiedByRoad(cellPos.x, cellPos.y - 1)) { id += 2;}                           // LEFT
        if (grid.IsTileOccupiedByRoad(cellPos.x + 1, cellPos.y)) { id += 4;}                           // DOWN
        if (grid.IsTileOccupiedByRoad(cellPos.x, cellPos.y + 1)) { id += 8;}                           // RIGHT
        
        if (id == 0)
        {
            // There needs to be an adjacent road or have the drag control.
            id = 1;
            //return;
        }

        Destroy(currentState);
        currentState = Instantiate(statePrefabs[id], transform.position, Quaternion.Euler(statePrefabs[id].transform.localEulerAngles), transform);
    }
}