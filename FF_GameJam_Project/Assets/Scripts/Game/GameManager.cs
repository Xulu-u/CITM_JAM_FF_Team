using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct HouseBase
{
    Vector2Int position;
    HouseType type;
}

public struct FactoryRound
{
    // Position
    // Type
}

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Time Control")]
    public float timePerRound;
    private float currentTime;

    [Header("Spawnables")]
    public List<GameObject> factoryPrefabs;
    public List<GameObject> housePrefabs;
    public List<GameObject> factoryCanvas = new List<GameObject>();

    public List<Vector2Int> existingFactories = new List<Vector2Int>();
    
    [Header("Camera")]
    public Camera ortographicCamera;

    [Header("Rounds")]

    [HideInInspector]
    GameGrid gameGrid;
    public int completedTravel = 0;

    
    public 

    void Start()
    {
        GameObject gridGO = GameObject.Find("GameGrid");
        gameGrid = gridGO.GetComponent<GameGrid>();

        currentTime = timePerRound;
        TutorialRound();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime <= 0.0f)
        {
            RoundEnd();
            currentTime = timePerRound;
        }

        foreach (GameObject obj in factoryCanvas)
        {
            if (obj.GetComponentInChildren<FactoryCounter>().packetTimer <= 0.0f)
            {
                obj.GetComponentInChildren<FactoryCounter>().packetCount++;
                obj.GetComponentInChildren<FactoryCounter>().packetTimer = obj.GetComponentInChildren<FactoryCounter>().packetTime;
            }
            obj.GetComponentInChildren<FactoryCounter>().packetTimer -= Time.deltaTime;
            obj.GetComponentInChildren<Text>().text = obj.GetComponentInChildren<FactoryCounter>().packetCount.ToString();

            if(obj.GetComponentInChildren<FactoryCounter>().packetCount >= 8)
            {
                SceneManager.LoadScene("Lose");
            }
        }

        if (factoryCanvas.Count >= 14)
        {
            SceneManager.LoadScene("Win");
        }
    }

    private void TutorialRound()
    {
        //Spawn one factory
        SpawnOnAvailableTile("Factory");

        //Spawn one house
        SpawnOnAvailableTile("House");
    }

    private void RoundEnd()
    {
        SpawnFactories();
        SpawnHouse();
    }

    private void SpawnFactories()
    {
        SpawnOnAvailableTile("Factory");
    }

    private void SpawnHouse()
    {
        int numberOfHouses = Random.Range(0, 2) + 1;

        for(int i = 0; i < numberOfHouses; ++i)
        {
            SpawnOnAvailableTile("House");
        }
    }

    public void SpawnOnAvailableTile(string type)
    {
        if (type == "Factory")
        {
            Vector3Int factTile = gameGrid.GetRandomFactoryTile();
            Vector2Int tilePos  = new Vector2Int(factTile.x, factTile.y);
            bool isHorizontal   = (factTile.z == 0) ? true : false;
            
            Vector3 worldPos    = gameGrid.GetWorldPositionFromGrid(tilePos);
            Quaternion rotation = Quaternion.identity;
            if (isHorizontal)
            {
                worldPos += new Vector3(0.0f, 0.0f, 10.0f);
            }
            else
            {
                worldPos += new Vector3(10.0f, 0.0f, 20.0f);
                rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }

            GameObject obj = Instantiate(factoryPrefabs[Random.Range(0, factoryPrefabs.Count - 1)], worldPos, rotation);
            factoryCanvas.Add(obj);
            obj.GetComponentInChildren<Canvas>().worldCamera = ortographicCamera;
            
            existingFactories.Add(tilePos);

            gameGrid.SetTileWalkable(tilePos.x, tilePos.y, TileType.END_COAL);
            gameGrid.SetEntity(tilePos.x, tilePos.y, TileFunctionality.BRIDGE);

            // Later on for roads check if the factory's position is in the tile list or sth idk.
        }
        if (type == "House")
        {
            Vector2Int tilePos  = gameGrid.GetRandomEmptyTile();
            Vector3 worldPos    = gameGrid.GetWorldPositionFromGrid(tilePos);
            House newHouse = Instantiate(housePrefabs[Random.Range(0, housePrefabs.Count - 1)], new Vector3(worldPos.x + 10, 0.0f, worldPos.z + 10), Quaternion.identity).GetComponent<House>();

            newHouse.startPoint = tilePos;
            newHouse.endPoint = existingFactories[existingFactories.Count - 1];
            newHouse.path = gameGrid.path;
            newHouse.grid = gameGrid;

            gameGrid.SetTileWalkable(tilePos.x, tilePos.y, TileType.START_COAL);
            gameGrid.SetEntity(tilePos.x, tilePos.y, TileFunctionality.BRIDGE);
        }
    }
}
