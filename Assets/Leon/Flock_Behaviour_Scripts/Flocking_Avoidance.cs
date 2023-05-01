using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flocking/Behaviour/Avoidance")]
public class Flocking_Avoidance: Flocking_Behavior
{
    // Start is called before the first frame update
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller, Transform Leader)
    {
        if (World_Context.Count == 0)
        {//No Changes
            return Current_Agent.transform.forward;
        }

        Vector3 Avoidance_Adjust = Vector3.zero;
        int Avoid_Num = 0;
        foreach (Transform Neighbour in World_Context)
        {
            if(Vector3.Distance(Neighbour.transform.position,Current_Agent.transform.position) >= Controller.Square_Avoidance_Radius_Access)
            {
                Avoidance_Adjust += Current_Agent.transform.position - Neighbour.transform.position;
                Avoid_Num++;
            }
            if(Avoid_Num<0)
            {
                Avoidance_Adjust /= Avoid_Num;
            }
            
        }
        return Avoidance_Adjust;
    }
}
