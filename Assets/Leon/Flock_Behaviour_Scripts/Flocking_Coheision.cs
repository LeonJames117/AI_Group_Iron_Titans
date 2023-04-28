using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flocking/Behaviour/Coheision")]


public class Flocking_Coheision : Flocking_Behavior
{
    Vector3 Smoothing_Vel;
    public float Coheision_Smoohting = 0.5f;
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller, Flocking_Leader Leader)
    {
       if(World_Context.Count==0)
        {
            return Vector3.zero;
        }

        Vector3 Cohesion_Adjust = Vector3.zero;
        foreach(Transform Neighbour in World_Context)
        {
            Cohesion_Adjust += Neighbour.position;
        }

        Cohesion_Adjust /= World_Context.Count;
        Cohesion_Adjust-= Current_Agent.transform.position;
        Cohesion_Adjust = Vector3.SmoothDamp(Current_Agent.transform.forward, Cohesion_Adjust, ref Smoothing_Vel, Coheision_Smoohting);
        return Cohesion_Adjust;
    }
   

}
