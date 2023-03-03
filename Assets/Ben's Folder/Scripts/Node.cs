using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IBinaryHeapNode<Node>
{
    public Vector3 position;
    public bool isObstructed = false;

    public int hCost;
    public int gCost;

    public int xGridCoord;
    public int yGridCoord;

    public Node parent;

    int heapIndex;

    public Node(Vector3 p_position, bool p_isObstructed) 
    {
        position = p_position;
        isObstructed = p_isObstructed;
    }

    public int fCost 
    {
        get 
        {
            return gCost + hCost;
        }
    }

    public int nodeIndex 
    {
        get 
        {
            return heapIndex;
        }

        set 
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node compareNode) 
    {
        int diff = fCost - compareNode.fCost;

        if(diff == 0) 
        {
            diff = hCost - compareNode.hCost;
            if(diff == 0) 
            {
                diff = -1;
            }
        }
        diff /= Mathf.Abs(diff);

        //returns 1 if the compare node is higher
        //returns -1 if the comapare node is lower
        return -diff;
    }
}
