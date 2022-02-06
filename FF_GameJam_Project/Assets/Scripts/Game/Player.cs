using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    GameGrid gameGrid       = null;
    gridCell hoveredCell    = null;

    Vector2Int prevHover    = Vector2Int.zero;
    bool draggingLeft       = false;
    bool firstTileLeft      = false;
    bool draggingRight      = false;
    bool firstTileRight     = false;

    private int LMB = 0;                                                                                // Left Mouse Button
    private int RMB = 1;                                                                                // Right Mouse Button

    [SerializeField] private LayerMask  whatIsAGridLayer;
    [SerializeField] private Camera     mainCamera;
    [SerializeField] private GameObject freeCam;
    [SerializeField] private GameObject editorCamera;
    [SerializeField] private GameObject roadTile;
    [SerializeField] private GameObject redGrid;

    public AudioManagerEffects audioManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if(editorCamera.active == true)
        {
            hoveredCell = IsMouseOverAGridSpace();
            if (hoveredCell != null)
            {
                bool onNewCell = (prevHover != hoveredCell.GetPosition());
                if (onNewCell)
                {
                    //Debug.Log("Mouse is hovering " + hoveredCell.GetPosition() + " cell.");
                    gameGrid.IsTileOccupied(hoveredCell.GetPosition().x, hoveredCell.GetPosition().y);
                    prevHover = hoveredCell.GetPosition();
                }

                if (Input.GetMouseButtonDown(LMB))                  { draggingLeft = true; firstTileLeft = true; }
                if (Input.GetMouseButtonUp(LMB))                    { draggingLeft = false; }
                if (draggingLeft && (onNewCell || firstTileLeft))   { CreateRoadTile(hoveredCell); firstTileLeft = false; }

                if (Input.GetMouseButtonDown(RMB))                  { draggingRight = true; firstTileRight = true; }
                if (Input.GetMouseButtonUp(RMB))                    { draggingRight = false; }
                if (draggingRight && (onNewCell || firstTileRight)) { DestroyRoadTile(hoveredCell); firstTileRight = false; }
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ToggleFreeCam();
        }
    }

    private gridCell IsMouseOverAGridSpace()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return (Physics.Raycast(ray, out RaycastHit hitInfo, 505f, whatIsAGridLayer) ? hitInfo.transform.GetComponent<gridCell>() : null);
    }

    void CreateRoadTile(gridCell clickedCell)
    {   
        Vector2Int pos = clickedCell.GetPosition();
        if (gameGrid.IsTileOccupied(pos.x, pos.y))
        {
            return;
        }

        GameObject newTile = InstantiateRoadTile(pos);
        UpdateGridMaps(pos, clickedCell, newTile);
    }

    void DestroyRoadTile(gridCell clickedCell)
    {
        Vector2Int pos = clickedCell.GetPosition();
        Tile tile = gameGrid.GetTile(pos.x, pos.y);
        if (tile == null)
        {
            Debug.Log("CANNOT DESTROY A TILE THAT DOES NOT EXIST");
            return;
        }

        Destroy(tile.gameObject);
        UpdateGridMaps(pos, null, null);
        
        //clickedCell.GetComponentInChildren<SpriteRenderer>().material.color = Color.black;
    }

    GameObject InstantiateRoadTile(Vector2Int pos)
    {
        Vector3 spawnPos = gameGrid.GetWorldPositionFromGrid(pos);
        return Instantiate(roadTile, new Vector3(spawnPos.x + 10, 0.5f, spawnPos.z + 10), Quaternion.identity);
    }

    void UpdateGridMaps(Vector2Int pos, gridCell cell, GameObject newTile)
    {

        if (newTile != null)
        {
            gameGrid.SetTileWalkable(pos.x, pos.y);                                                     //turn this tile into walkable, usefull in the future to spawn roads
            gameGrid.SetEntity(pos.x, pos.y, TileFunctionality.ROAD);

            Tile tile = newTile.GetComponent<Tile>();
            tile.grid = gameGrid;
            tile.cell = cell;
            gameGrid.SetTile(pos.x, pos.y, tile);

            audioManagerScript.PlayRoadBuild();
        }
        else
        {
            gameGrid.SetTileWalkable(pos.x, pos.y, false);                                              //turn this tile into walkable, usefull in the future to spawn roads
            gameGrid.SetEntity(pos.x, pos.y, TileFunctionality.EMPTY);
            gameGrid.SetTile(pos.x, pos.y, null);

            audioManagerScript.PlayRoadBuild();                                                         // Put audioManagerScript.PlayRoadDestroy();
        }
    }

    void ToggleFreeCam()
    {
        if(editorCamera.active == true)
        {
            freeCam.SetActive(true);
            editorCamera.SetActive(false);
            redGrid.SetActive(false);
        }
        else
        {
            freeCam.SetActive(false);
            editorCamera.SetActive(true);
            redGrid.SetActive(true);
        }
    }
}
