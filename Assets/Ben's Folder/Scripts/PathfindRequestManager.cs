using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindRequestManager : MonoBehaviour
{
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

    public Vector3[] RequestPath() 
    {
        Vector3[] result = new Vector3[4];
        return result;
    }


}
