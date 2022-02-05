using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;

    private GridPathfinding grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height)
    {
        grid = new GridPathfinding(height, width, 20f, Vector3.zero);
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetWidth(); ++x)
        {
            for(int y = 0; y < grid.GetHeight(); ++y)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetlowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Reached final node!!!!
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

        }

        //out of nodes on the open list
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if(currentNode.getNodePosition().x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode.getNodePosition().x - 1, currentNode.getNodePosition().y));
        }

        if(currentNode.getNodePosition().x + 1 < grid.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(currentNode.getNodePosition().x + 1, currentNode.getNodePosition().y));
        }

        if (currentNode.getNodePosition().y - 1 >= 0)
        {
            //DOWN
            neighbourList.Add(GetNode(currentNode.getNodePosition().x, currentNode.getNodePosition().y - 1));
        }

        if (currentNode.getNodePosition().y + 1 < grid.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(currentNode.getNodePosition().x, currentNode.getNodePosition().y + 1));
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);

        PathNode currentNode = endNode;

        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        Vector2Int positionA = a.getNodePosition();
        Vector2Int positionB = b.getNodePosition();
        int xDistance = Mathf.Abs(positionA.x - positionB.x);
        int yDistance = Mathf.Abs(positionA.y - positionB.y);

        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_STRAIGHT_COST * remaining;

    }

    private PathNode GetlowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for(int i = 1; i< pathNodeList.Count; ++i)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }


    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x,y);
    }
}
