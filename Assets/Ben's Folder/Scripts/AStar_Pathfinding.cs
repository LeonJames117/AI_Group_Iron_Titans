using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Pathfinding : MonoBehaviour
{
    [SerializeField] Nav_Grid grid;
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();

    Vector2 startNodeIndicies;
    Vector2 endNodeIndicies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pathfind(Vector3 startPos, Vector3 destinationPos) 
    {
        startNodeIndicies = CalculateGridPoint(startPos);
        print("s_node: " + startNodeIndicies);
        endNodeIndicies = CalculateGridPoint(destinationPos);
        print("e_node: " + endNodeIndicies);
    }

    Vector2 CalculateGridPoint(Vector3 pos) 
    {
        print("pos: " + pos);
        float x = (int)(pos.x - grid.bottomLeft_GridPos.x);
        float y = (int)(pos.z - grid.bottomLeft_GridPos.z);

        print("x1: " + x);
        print("y1: " + y);

        x = (int)(x / grid.nodeDiameter) + 1;
        y = (int)(y / grid.nodeDiameter) + 1;

        print("x2: " + x);
        print("y2: " + y);

        return new Vector2(x, y);
    }
}
