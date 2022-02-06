using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    [HideInInspector] public List<PathNode> currentPath;     //Path A-B
    [HideInInspector] public List<PathNode> reversePath;     //Path B-A

    [Header("Car Behavior values")]
    public float moveSpeed;

    private GameGrid grid;
    private GameManager game;
    private PathNode nextCheckPoint;
    private int idFoward = 0;
    private int idBackward = 0;


    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GameGrid").GetComponent<GameGrid>();
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentPath = reversePath;
        reversePath.Reverse();
        nextCheckPoint = currentPath[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOnDestination(nextCheckPoint))
        {
            if (idFoward < currentPath.Count - 1)
                nextCheckPoint = currentPath[idFoward++];
            else if (idBackward < reversePath.Count - 1)
                nextCheckPoint = reversePath[idBackward++];
            else
            {
                game.completedTravel++;
                Destroy(this);
            }
        }
        MoveCar();
    }

    private void MoveCar()
    {
        Vector3 direction = this.transform.position - grid.GetWorldPositionFromGrid(nextCheckPoint.getNodePosition());
        Vector3 newPosition = direction * (moveSpeed * Time.deltaTime);
        transform.position += newPosition;
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
