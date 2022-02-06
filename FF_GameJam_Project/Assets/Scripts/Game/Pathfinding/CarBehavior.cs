using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public class CarBehavior : MonoBehaviour
{
    [HideInInspector] public List<PathNode> currentPath;     //Path A-B
    [HideInInspector] public List<PathNode> reversePath;     //Path B-A

    [Header("Car Behavior values")]
    public float moveSpeed;
    public float rotationSpeed = 10f;

    private GameGrid grid;
    private GameManager game;
    [HideInInspector] public PathNode nextCheckPoint;
    [HideInInspector] public PathNode currentCheckPoint;
    private int idFoward = 1;
    private int idBackward = 0;
    private direction currentDir;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        //currentPath = new List<PathNode>();
        reversePath = new List<PathNode>(currentPath);
        reversePath.Reverse();
    }

    // Update is called once per frame



    //no va perque li poses el target position a la segona posicio de la llista, pero el count del forward no l'avances
    void LateUpdate()
    {
        if(IsOnDestination(nextCheckPoint))
        {
            if (idFoward < currentPath.Count - 1)
            {
                currentCheckPoint = nextCheckPoint;
                nextCheckPoint = currentPath[idFoward++];
            }
            else if (idBackward < reversePath.Count - 1)
            {
                currentCheckPoint = nextCheckPoint;
                nextCheckPoint = reversePath[idBackward++];
            }
            else
            {
                game.completedTravel++;
                Destroy(gameObject);
            }
        }

        MoveCar();
    }

    private void MoveCar()
    {

        offset = GetOffsetFromDirection();
        Debug.Log("Offset ->" + offset + "direction: " + currentDir);
        Vector3 carPosition = transform.position;
        Vector3 targetPosition = grid.GetWorldPositionFromGrid(nextCheckPoint.getNodePosition()) + offset;



        transform.LookAt(targetPosition);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
        


        transform.position = Vector3.MoveTowards(carPosition, Vector3.Lerp(carPosition, targetPosition, 0.05f), moveSpeed);
        


        //transform.position += newPosition;
    }

    public bool IsOnDestination(PathNode nextPoint)
    {
        offset = GetOffsetFromDirection();
        Debug.Log("Offset CheckDestination ->" + offset + "direction: " + currentDir);
        Vector2Int pos = nextPoint.getNodePosition();
        Vector3 destination = grid.GetWorldPositionFromGrid(pos) + offset;

        if (Vector3.Distance(gameObject.transform.position, destination) < 2.5f)
            return true;
        return false;

    }

    private Vector3  GetOffsetFromDirection()
    {
        if (nextCheckPoint.getNodePosition().x > currentCheckPoint.getNodePosition().x)
        {
            currentDir = direction.DOWN;
        }
        if (nextCheckPoint.getNodePosition().x < currentCheckPoint.getNodePosition().x)
        {
            currentDir = direction.UP;
        }
        if (nextCheckPoint.getNodePosition().y > currentCheckPoint.getNodePosition().y)
        {
            currentDir = direction.RIGHT;
        }
        if (nextCheckPoint.getNodePosition().y < currentCheckPoint.getNodePosition().y)
        {
            currentDir = direction.LEFT;
        }

        switch (currentDir)
        {
            case direction.LEFT: { offset = new Vector3(6f, 0.5f, 10f); } break;
            case direction.RIGHT: { offset = new Vector3(14f, 0.5f, 10f); } break;
            case direction.UP: { offset = new Vector3(10f, 0.5f, 14f); } break;
            case direction.DOWN: { offset = new Vector3(10f, 0.5f, 6f); } break;
        }

        return offset;
    }

    private float GetDistanceFromDirection()
    {
        float offset = 0;

        switch (currentDir)
        {
            case direction.LEFT: { offset = 5; } break;
            case direction.RIGHT: { offset = -5; } break;
            case direction.UP: { offset = 5; } break;
            case direction.DOWN: { offset = -5; } break;
        }

        return offset;
    }
}
