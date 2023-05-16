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
        {//No agents in detection radius so no changes needed
            return Current_Agent.transform.forward;
        }

        Vector3 Avoidance_Adjust = Vector3.zero;
        int Avoid_Num = 0;
        foreach (Transform Neighbour in World_Context)
        {// For every neighbour in detection radius
            if(Vector3.Distance(Neighbour.transform.position,Current_Agent.transform.position) >= Controller.Square_Avoidance_Radius_Access)
            {// If the distance between the 2 agents is higher than or equal to the square of the avoidance radius, add the distance between the 2 minus the y coordanate to the adjustment
                Vector3 Current_AgentNoY = new Vector3(Current_Agent.transform.position.x, 0, Current_Agent.transform.position.z);
                Vector3 NeighbourNoY = new Vector3(Neighbour.transform.position.x, 0, Neighbour.transform.position.z);
                Avoidance_Adjust += Current_AgentNoY - NeighbourNoY;
                Avoid_Num++;
            }
            
            
        }
        if (Avoid_Num > 0)
        {// If any adjustment needs to be made, find an adverage of the adjustment needed per agent in detection
            Avoidance_Adjust /= Avoid_Num;
        }
        return Avoidance_Adjust;
    }
}
