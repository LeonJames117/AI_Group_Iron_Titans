using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flocking/Behaviour/Coheision")]


public class Flocking_Coheision : Flocking_Behavior
{
    Vector3 Smoothing_Vel;
    public float Coheision_Smoohting = 0.5f;
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller, Transform Leader)
    {
       if(World_Context.Count==0)
        {//No agents in detection radius so no changes needed
            return Vector3.zero;
        }

        Vector3 Cohesion_Adjust = Vector3.zero;
        foreach(Transform Neighbour in World_Context)
        {// For each agent in detection radius, add their positon vector to the adjustment
            Cohesion_Adjust += Neighbour.position;
        }
        // Get an adverage of the adjustment per agent, find the differnce between the adjusment and the agents positon, smooth the vector so that the movement is smoother
        Cohesion_Adjust /= World_Context.Count;
        Cohesion_Adjust -= Current_Agent.transform.position;
        Cohesion_Adjust = Vector3.SmoothDamp(Current_Agent.transform.forward, Cohesion_Adjust, ref Smoothing_Vel, Coheision_Smoohting);
        return Cohesion_Adjust;
    }
   

}
