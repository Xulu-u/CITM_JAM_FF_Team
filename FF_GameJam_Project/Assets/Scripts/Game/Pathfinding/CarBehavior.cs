using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int idFoward = 0;
    private int idBackward = 0;


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
    void LateUpdate()
    {
        if(IsOnDestination(nextCheckPoint))
        {
            if (idFoward < currentPath.Count - 1)
            {
                nextCheckPoint = currentPath[idFoward++];
                Vector2Int debug = nextCheckPoint.getNodePosition();
                Vector3 AAA = grid.GetWorldPositionFromGrid(debug);
                Debug.Log("Tile ->" + nextCheckPoint.ToString());
                Debug.Log("World Pos -> X: " + AAA.x + "Z: " + AAA.z);
            }
            else if (idBackward < reversePath.Count - 1)
                nextCheckPoint = reversePath[idBackward++];
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
        Vector3 carPosition = transform.position;
        Vector3 targetPosition = grid.GetWorldPositionFromGrid(nextCheckPoint.getNodePosition());

        Quaternion rotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation,5000f);
        
        transform.position = Vector3.MoveTowards(carPosition, Vector3.Lerp(carPosition, targetPosition, 0.05f), moveSpeed);
        
        //transform.position += newPosition;
    }

    public bool IsOnDestination(PathNode nextPoint)
    {
        Vector2Int pos = nextPoint.getNodePosition();
        Vector3 destination = grid.GetWorldPositionFromGrid(pos);

        if (Vector3.Distance(gameObject.transform.position, destination) < 0.5f)
            return true;
        return false;

    }
}
