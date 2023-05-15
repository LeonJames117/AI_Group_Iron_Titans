using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flocking/Behaviour/Follow")]
public class Flocking_Follow : Flocking_Behavior
{

    
    public float Closest_Distance = 5f;
    public float Max_Radius = 10f;
    // Start is called before the first frame update
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller, Transform Leader)
    {

        Vector3 Leader_Offset = Leader.transform.position - Current_Agent.transform.position;
        float Currernt_Distance = Leader_Offset.magnitude/ Max_Radius; // 0 = Leaders positon, 1 = Edge of follow radius
        if(Currernt_Distance < Closest_Distance)
        {
            return Vector3.zero;
        }
        return Leader_Offset * Currernt_Distance;
    }
}
