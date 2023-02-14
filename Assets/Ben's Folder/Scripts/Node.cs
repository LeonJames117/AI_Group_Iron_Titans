using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public bool isObstructed = false;

    public Node(Vector3 p_position, bool p_isObstructed) 
    {
        position = p_position;
        isObstructed = p_isObstructed;
    }
}
