using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Controller : MonoBehaviour
{
    //List Setup
    public Flocking_Agent Flocking_Prefab;
    List<Flocking_Agent> All_Agents = new List<Flocking_Agent>();
    //Behavior
    public Flocking_Behavior Behavior;
    //Spawning
    [Range(1, 500)]
    public int Agent_Count = 30;
    const float Density = 0.5f;
    //Movement
    [Range(1f, 100f)]
    public float Speed_Mulitplier=10f;
    [Range(1f, 100f)]
    public float Max_Speed = 5f;
    float Square_Max_Speed;
    //Neighbour Detection and avoidance
    [Range(1f, 10f)]
    public float Detection_Radius= 1.5f;
    [Range(0f, 1f)]
    public float Avoidance_Radius= 0.5f;
    float Square_Detection_Radius;
    float Square_Avoidance_Radius;
    public float Square_Avoidance_Radius_Access { get { return Square_Avoidance_Radius; } }
    // Start is called before the first frame update
    void Start()
    {
        //Square Variable setup
        Square_Max_Speed = Max_Speed * Max_Speed;
        Square_Detection_Radius = Detection_Radius * Detection_Radius;
        Square_Avoidance_Radius = Square_Detection_Radius * Avoidance_Radius * Avoidance_Radius;

        //Setup Flock
        for(int i = 0; i < Agent_Count; i++)
        {
            Flocking_Agent New_Agent = Instantiate(Flocking_Prefab,Random.insideUnitCircle * Agent_Count * Density,Quaternion.Euler(Vector3.forward*Random.Range(0f,360f)),transform);
            New_Agent.name = "Flocking_Agent " + i;
            All_Agents.Add(New_Agent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        

            
        
        
    }
}
