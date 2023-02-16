using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Nav_Grid : MonoBehaviour
{
    [SerializeField] LayerMask obstacle_LayerMask;
    [SerializeField] Vector2 gridSize;

    [Range(0.1f, 10.0f)]
    [SerializeField] float nodeRadius;

    [SerializeField] float updateFrequency;

    [Range(0.0f, 1.0f)]
    [SerializeField] float visualisationAlpha;

    float nodeDiameter;

    Node[,] grid;
    int gridSizeX;
    int gridSizeY;

    Vector3 nodeHalfExtents;
    Vector3 GizmoNodeSize;

    bool updateNavGrid = true;
    float updateTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    void InitGrid()
    { 
        InitGridParameters();

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft_GridPos = transform.position - 
                                    (transform.right * gridSize.x / 2) - 
                                    (transform.forward * gridSize.y / 2); 

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 node_position = bottomLeft_GridPos +
                                        (Vector3.right * ((x * nodeDiameter) + nodeRadius)) +
                                        (Vector3.forward * ((y * nodeDiameter) + nodeRadius));

                bool isObstructed = Physics.CheckBox(node_position, nodeHalfExtents, this.transform.rotation, obstacle_LayerMask);

                grid[x, y] = new Node(node_position, isObstructed);
            }
        }
    }

    void InitGridParameters() 
    {
        nodeDiameter = nodeRadius * 2;
        nodeHalfExtents = new Vector3(nodeRadius, nodeRadius, nodeRadius);
        GizmoNodeSize = new Vector3(nodeDiameter - 0.1f, 0.9f, nodeDiameter - 0.1f);
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
    }

    void ClearGrid() 
    {
        grid = new Node[gridSizeX, gridSizeY];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if(grid != null) 
        {
            foreach(Node node in grid) 
            {
                if (node.isObstructed) 
                {
                    Gizmos.color = new Color(1, 0, 0, visualisationAlpha);    
                }
                else 
                {
                    Gizmos.color = new Color(0, 1, 0, visualisationAlpha);
                }

                Gizmos.DrawWireCube(node.position, new Vector3(nodeDiameter, 0.9f, nodeDiameter));

                Gizmos.DrawCube(node.position, GizmoNodeSize);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
        if(updateTimer >= updateFrequency) 
        {
            UpdateNavGrid();
            updateTimer = 0.0f;
        }
    }

    void UpdateNavGrid() 
    {
        ClearGrid();
        InitGrid();
    }
}
