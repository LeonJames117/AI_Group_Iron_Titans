using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Leader : MonoBehaviour
{
    public AStar_Pathfinding Pathing;
    public PlayerController controller;
    Nav_Agent NavA;

    // Start is called before the first frame update
    void Start()
    {
        NavA = GetComponent<Nav_Agent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // NavA.StartFollowPath(Pathing.Pathfind(transform.position, controller.transform.position));
    }
}
