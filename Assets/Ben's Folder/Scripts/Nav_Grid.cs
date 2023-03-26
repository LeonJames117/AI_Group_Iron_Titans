using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Nav_Grid : MonoBehaviour
{
    [SerializeField] LayerMask obstacle_LayerMask;
    LayerMask walkable_LayerMask;
    [SerializeField] Vector2 gridSize;

    [Range(0.1f, 10.0f)]
    [SerializeField] float nodeRadius;

    [SerializeField] float updateFrequency;

    [Range(0.0f, 1.0f)]
    [SerializeField] float visualisationAlpha;

    public float nodeDiameter;

    Node[,] grid;
    public int gridSizeX;
    public int gridSizeY;

    Vector3 nodeHalfExtents;
    Vector3 GizmoNodeSize;

    bool updateNavGrid = true;
    float updateTimer = 0.0f;

    public Vector3 bottomLeft_GridPos;
    public TerrainType[] terrainTypes;

    public List<Node> path;
    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    void InitGrid()
    {

        InitWalkableLayerMask();
        InitGridParameters();

        grid = new Node[gridSizeX, gridSizeY];
        bottomLeft_GridPos = transform.position - 
                             (transform.right * gridSize.x / 2) - 
                             (transform.forward * gridSize.y / 2); 

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 node_position = bottomLeft_GridPos +
                                        (Vector3.right * ((x * nodeDiameter) + nodeRadius)) +
                                        (Vector3.forward * ((y * nodeDiameter) + nodeRadius));

                bool isObstructed = Physics.CheckBox(node_position, nodeHalfExtents, this.transform.rotation, obstacle_LayerMask) || 
                                    !Physics.CheckBox(node_position, nodeHalfExtents, this.transform.rotation, walkable_LayerMask);


                int weight = 0;
                RaycastHit hit;
                Vector3 raycastStart = new Vector3(node_position.x, node_position.y + 5, node_position.z);

                if (Physics.Raycast(raycastStart, Vector3.down, out hit, Mathf.Infinity, walkable_LayerMask)) 
                {
                    weight = GetLayerWeight(hit.collider.gameObject.layer);
                    //Debug.DrawRay(raycastStart, Vector3.down * 10.0f, Color.blue);
                }

                grid[x, y] = new Node(node_position, isObstructed, weight);

                grid[x, y].xGridCoord = x;
                grid[x, y].yGridCoord = y;
            }
        }
    }

    void InitWalkableLayerMask() 
    {
        foreach (TerrainType terrainType in terrainTypes)
        {
            walkable_LayerMask |= terrainType.layerMask;
        }
    }

    int GetLayerWeight(LayerMask layer) 
    {
        foreach (TerrainType terrainType in terrainTypes)
        {
            if(layer.value == Mathf.Log(terrainType.layerMask.value, 2)) 
            {
                return terrainType.weight;
            }
        }

        return 0;
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

                if(path != null) 
                {
                    if (path.Contains(node)) 
                    {
                        Gizmos.color = new Color(1, 0, 1, visualisationAlpha);
                    }                    
                }

                Gizmos.DrawWireCube(node.position, new Vector3(nodeDiameter, 0.9f, nodeDiameter));
                Gizmos.DrawCube(node.position, GizmoNodeSize);
            }
        }
    }

    public Node GetNodeFromPosition(Vector3 position) 
    {
        float x = (int)(position.x - bottomLeft_GridPos.x);
        float y = (int)(position.z - bottomLeft_GridPos.z);

        x = (int)(x / nodeDiameter);
        y = (int)(y / nodeDiameter);

        return grid[(int)x, (int)y];
    }

    public List<Node> GetNeighbours(Node node, Node endNode) 
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int xCoord = node.xGridCoord + x;
                int yCoord = node.yGridCoord + y;

                if ((x == 0 && y == 0)) 
                {
                    continue;
                }
                
                if(((!grid[xCoord, yCoord].isObstructed) ||(grid[xCoord, yCoord] == endNode)) &&
                   (xCoord >= 0 && xCoord <= gridSizeX) &&
                   (yCoord >= 0 && xCoord <= gridSizeY)) 
                {
                    neighbours.Add(grid[xCoord, yCoord]);
                }
            }
        }

        return neighbours;
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

[System.Serializable]
public class TerrainType
{
    [SerializeField] public LayerMask layerMask;
    [SerializeField] public int weight;
}
