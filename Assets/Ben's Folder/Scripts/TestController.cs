using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] AStar_Pathfinding pathfinder;

    [SerializeField] GameObject startObj;
    [SerializeField] GameObject endObj;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            pathfinder.Pathfind(startObj.transform.position, endObj.transform.position);
        }
    }
}
