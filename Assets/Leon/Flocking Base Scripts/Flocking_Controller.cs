using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Controller : MonoBehaviour
{
    //List Setup
    public Flocking_Agent Flocking_Prefab;
    public List<Flocking_Agent> All_Agents = new List<Flocking_Agent>();
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

    public Flocking_Leader Flock_Lead;

    
    
    // Start is called before the first frame update
    void Start()
    {
        //Square Variable setup
        Square_Max_Speed = Max_Speed * Max_Speed;
        Square_Detection_Radius = Detection_Radius * Detection_Radius;
        Square_Avoidance_Radius = Square_Detection_Radius * Avoidance_Radius * Avoidance_Radius;


        //Pathfinding
        //

        //Setup Flock
        //for (int i = 0; i < Agent_Count; i++)
        //{
        //    Vector3 Random_Pos_in_Cir = Random.insideUnitCircle * Agent_Count * Density;

            
        //    if (!Physics.CheckSphere(Random_Pos_in_Cir, 0.5f))
        //    {
        //        Flocking_Agent New_Agent = Instantiate(Flocking_Prefab,Random_Pos_in_Cir , Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), transform);
        //        print("Agent " + i + " Instaticated");
        //        New_Agent.name = "Flocking_Agent " + i;
        //        print("Agent " + i + " Added to array");
        //        All_Agents.Add(New_Agent);
        //    }
        //    else
        //    {
        //        print("Overlap detected on " + "Agent " + i);
        //        i--;
        //    }
            
            
            
      //  }
    }

    // Update is called once per frame
    void Update()
    {

        //NavA.StartFollowPath(Pathing.Pathfind(this.transform.position,FindObjectOfType<PlayerController>().transform.position));

        foreach (var agent in All_Agents)
        {
            //Finds all neighbours in detection raduis
            List<Transform> World = GetNearbyObjects(agent);
            Vector3 Move = Behavior.Caculate_Movement(agent, World, this, Flock_Lead);
            Move *= Speed_Mulitplier;
            if (Move.sqrMagnitude > Square_Max_Speed)
            {
                //Reset move to 1 and multiply by max speed
                Move = Move.normalized * Max_Speed;
            }
            agent.Move_To(Move);
        }




    }


    List<Transform> GetNearbyObjects(Flocking_Agent A)
    {
        List<Transform> Nearby = new List<Transform>();
        Collider[] NearbyCheck = Physics.OverlapSphere(A.transform.position, Detection_Radius);
        foreach(Collider Collider in NearbyCheck)
        {
            if(Collider != A.Agent_Collider_Access)
            {
                Nearby.Add(Collider.transform);
            }
        }
        return Nearby;
    }

}
