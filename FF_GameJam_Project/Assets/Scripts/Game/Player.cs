using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    GameGrid gameGrid;

    private int LMB = 0;                                                                                // Left Mouse Button
    private int RMB = 1;                                                                                // Right Mouse Button

    [SerializeField] private LayerMask  whatIsAGridLayer;
    [SerializeField] private Camera     mainCamera;
    [SerializeField] private GameObject freeCam;
    [SerializeField] private GameObject editorCamera;
    [SerializeField] private GameObject roadTile;

    Vector2Int prevHover = Vector2Int.zero;

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
            gridCell hoveredCell = IsMouseOverAGridSpace();
            if (hoveredCell != null)
            {
                if (prevHover != hoveredCell.GetPosition())
                {
                    Debug.Log("Mouse is hovering " + hoveredCell.GetPosition() + " cell.");
                    prevHover = hoveredCell.GetPosition();
                }

                if (Input.GetMouseButtonDown(LMB))  { InstantiateRoadTile(hoveredCell); }
                if (Input.GetMouseButtonDown(RMB))  { DestroyRoadTile(hoveredCell); }
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

    void InstantiateRoadTile(gridCell clickedCell)
    {   
        Vector2Int pos = Vector2Int.RoundToInt(clickedCell.GetPosition());
        // if (gameGrid.IsTileOccupied(pos.x, pos.y))
        // {
        //     return;
        // }

        Vector3 spawnPos    = gameGrid.GetWorldPositionFromGrid(pos);
        GameObject road     = Instantiate(roadTile, new Vector3(spawnPos.x + 10, 0.5f, spawnPos.z + 10), Quaternion.identity);

        gameGrid.SetTileWalkable(pos.x, pos.y);                                                   //turn this tile into walkable, usefull in the future to spawn roads
        gameGrid.SetEntity(pos.x, pos.y, TileFunctionality.ROAD);

        Tile tile = road.GetComponent<Tile>();
        tile.grid = gameGrid;
        tile.cell = clickedCell;
        gameGrid.SetTile(pos.x, pos.y, tile);
    }

    void DestroyRoadTile(gridCell clickedCell)
    {
        clickedCell.GetComponentInChildren<SpriteRenderer>().material.color = Color.black;
    }

    void ToggleFreeCam()
    {
        if(editorCamera.active == true)
        {
            freeCam.SetActive(true);
            editorCamera.SetActive(false);
        }
        else
        {
            freeCam.SetActive(false);
            editorCamera.SetActive(true);
        }
    }
}
