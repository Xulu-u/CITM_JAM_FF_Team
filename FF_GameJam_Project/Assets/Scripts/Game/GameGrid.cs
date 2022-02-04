using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType
{
    NON_WALKABLE,
    WALKABLE
}

public class GameGrid : MonoBehaviour
{
    public int height;
    public int width;
    private float gridSpaceSize = 20f;
    private Vector3 originPosition;

    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private GameObject originGrid;
    private GameObject[,] gameGrid;
    private TileType[,] walkabilityMap;
    // Start is called before the first frame update
    void Start()
    {
        originPosition = originGrid.transform.position;
        walkabilityMap = new TileType[height, width];
        CreateGrid();
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

    public bool IsTileOcupied(int x, int y)
    {
        return gameGrid[x, y].GetComponent<gridCell>().isOcupied;
    }
}
