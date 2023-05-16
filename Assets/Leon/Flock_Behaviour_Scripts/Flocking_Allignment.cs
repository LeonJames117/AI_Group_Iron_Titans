using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flocking/Behaviour/Allginment")]
public class Flocking_Allignment : Flocking_Behavior
{
    // Start is called before the first frame update
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller, Transform Leader)
    {
        if (World_Context.Count == 0)
        {//No other agent in detection radius, so no changes needed
            return Current_Agent.transform.forward;
        }

        Vector3 Alliggnment_Adjust = Vector3.zero;
        foreach (Transform Neighbour in World_Context)
        {// For each agent in detection radius, add their forward vector to the adjustment
            Alliggnment_Adjust += Neighbour.forward;
        }

        // Find and return an average of the adjustment
        Alliggnment_Adjust /= World_Context.Count;
        return Alliggnment_Adjust;
    }
}

