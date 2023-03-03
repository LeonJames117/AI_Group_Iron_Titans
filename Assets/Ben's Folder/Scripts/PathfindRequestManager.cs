using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindRequestManager : MonoBehaviour
{
    [SerializeField] AStar_Pathfinding pathfinder;
    public static PathfindRequestManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(instance);
        }
        else 
        {
            instance = this;
        }
    }

    public List<Vector3> RequestPath(Vector3 startPos, Vector3 endPos) 
    {
        List<Vector3> result = pathfinder.Pathfind(startPos, endPos);
        return result;   
    }
}
