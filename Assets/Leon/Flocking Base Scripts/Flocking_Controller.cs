using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Controller : MonoBehaviour
{
    //List Setup
    public List<Flocking_Agent> All_Agents = new List<Flocking_Agent>();
    //Behavior
    public Flocking_Behavior Behavior;
    //Spawning
    const float Density = 0.5f;
    //Movement
    [Range(1f, 100f)]
    public float Speed_Mulitplier=10f;
    [Range(1f, 100f)]
    public float Max_Speed = 5f;
    float Square_Max_Speed;
    //Neighbour Detection and avoidance
    [Range(0f, 10f)]
    public float Detection_Radius= 1.5f;
    [Range(0f, 10f)]
    public float Avoidance_Radius= 0.5f;
    float Square_Detection_Radius;
    float Square_Avoidance_Radius;
    public float Square_Avoidance_Radius_Access { get { return Square_Avoidance_Radius; } }

    public Transform Flock_Lead;

    
    
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


    public void CheckDead()
    {
        if(All_Agents.Count == 0)
        {
            Destroy(gameObject);
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
