using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Pathfinding : MonoBehaviour
{
    [SerializeField] Nav_Grid grid;

    Vector2 startNodeIndicies;
    Vector2 endNodeIndicies;

    public List<Vector3> Pathfind(Vector3 startPos, Vector3 destinationPos) 
    {
        Node startNode = grid.GetNodeFromPosition(startPos);
        Node endNode = grid.GetNodeFromPosition(destinationPos);

        BinaryHeap<Node> openHeap = new BinaryHeap<Node>(grid.gridSizeX * grid.gridSizeY);
        HashSet<Node> closedList = new HashSet<Node>();

        openHeap.Add(startNode);

        while (openHeap.nodeCount > 0) 
        {
            Node curNode = openHeap.Remove();
            closedList.Add(curNode);

            if(curNode == endNode) 
            {
                return TracePath(startNode, endNode);
            }

            foreach(Node neighbour in grid.GetNeighbours(curNode, endNode)) 
            {
                if (closedList.Contains(neighbour)) 
                {
                    continue;
                }

                int distBetweenNodes = CalculateDistanceBetweenNodes(curNode, neighbour);

                int new_gCost = curNode.gCost + distBetweenNodes + (distBetweenNodes / 10  * neighbour.weight);

                bool inOpenList = openHeap.Contains(neighbour);

                if (new_gCost < neighbour.gCost || !inOpenList) 
                {
                    neighbour.gCost = new_gCost;
                    neighbour.hCost = CalculateDistanceBetweenNodes(neighbour, endNode);
                    neighbour.parent = curNode;

                    if (!inOpenList) 
                    {
                        openHeap.Add(neighbour);
                    }
                    else 
                    {
                        openHeap.SortUp(neighbour);
                    }
                }
            }
        }

        return null;
    }

    List<Vector3> SimplifyPath(List<Node> path) 
    {
        List<Vector3> simplified_path = new List<Vector3>(); 

        //Add first waypoint
        simplified_path.Add(path[0].position);

        Vector3 directionA = Vector3.zero;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 directionB = Vector3.Normalize(path[i + 1].position - path[i].position);
            if (directionA != directionB)
            {
                //different
            }
            else 
            {
                simplified_path.RemoveAt(simplified_path.Count - 1);
            }

            simplified_path.Add(path[i + 1].position);
            directionA = directionB;
        }

        return simplified_path;
    }

    List<Vector3> TracePath(Node start, Node end) 
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while(current != start) 
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        grid.path = path;

        return SimplifyPath(path);
    }

    int CalculateDistanceBetweenNodes(Node start, Node end) 
    {
        int xDiff = Mathf.Abs(start.xGridCoord - end.xGridCoord);
        int yDiff = Mathf.Abs(start.yGridCoord - end.yGridCoord);

        int dist;

        if (xDiff > yDiff)
        {
            //dist = diagonal moves + direct moves
            dist = (14 * yDiff) + (10 * (xDiff - yDiff));
        }
        else
        {
            //dist = diagonal moves + direct moves
            dist = (14 * xDiff) + (10 * (yDiff - xDiff));
        }

        return dist;
    }
}
