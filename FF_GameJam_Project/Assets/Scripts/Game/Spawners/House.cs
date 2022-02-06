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
    private List<Vector2Int> factories;
    bool once = false;

    public float spawnTimeCar = 10.0f;
    private float currentTime;

    public House(Vector3 position, HouseType type)
    {
        this.position       = position;
        this.type           = type;
        this.path           = null;
        this.currentPath    = null;
    }

    public bool IsPathAvailable(Vector2Int start, Vector2Int end)
    {
        factories = GameObject.Find("GameManager").GetComponent<GameManager>().existingFactories;

        foreach(Vector2Int fact in factories)
        {
            currentPath = path.FindPath(start.x, start.y, fact.x, fact.y);
            if (currentPath != null)
                break;
        }
        return (currentPath != null);
    }

    private void Start()
    {
        currentTime = spawnTimeCar;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        //Debuging Path:
        if(currentTime <= 0.0f)
        {
            if (IsPathAvailable(startPoint, endPoint))
            {
                Vector2Int pos = currentPath[0].getNodePosition();
                Vector3 worldPos = grid.GetWorldPositionFromGrid(pos);

                GameObject car = Instantiate(carPrefab, new Vector3(worldPos.x, 0.5f, worldPos.z), Quaternion.Euler(0, 180, 0));
                car.GetComponent<CarBehavior>().currentPath = new List<PathNode>(currentPath);
                car.GetComponent<CarBehavior>().nextCheckPoint = currentPath[1];
                car.GetComponent<CarBehavior>().currentCheckPoint = currentPath[0];
                
            }
            currentTime = spawnTimeCar;
        }
        
    }
}
