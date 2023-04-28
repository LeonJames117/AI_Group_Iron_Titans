using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flocking/Behaviour/Together")]
public class Flocking_Together : Flocking_Behavior
{
    // Start is called before the first frame update
    public Flocking_Behavior[] Behaviors;
    public float[] Weights;
    public override Vector3 Caculate_Movement(Flocking_Agent Current_Agent, List<Transform> World_Context, Flocking_Controller Controller,Flocking_Leader Leader)
    {
        if(Behaviors.Length != Weights.Length)
        {
            Debug.LogError("Different Wieghts and Behavior count in together");
            return Vector3.zero;
        }

        Vector3 Final_Move = Vector3.zero;

        //Itterate Behaviors

        for(int i = 0; i < Behaviors.Length; i++)
        {
            Vector3 Move_IP = Behaviors[i].Caculate_Movement(Current_Agent, World_Context, Controller,Leader) * Weights[i];

            if(Move_IP!= Vector3.zero)
            {
                if(Move_IP.sqrMagnitude > Weights[i]*Weights[i])
                {
                    Move_IP.Normalize();
                    Move_IP*=Weights[i];
                }
                Final_Move += Move_IP;
            }
        }

        Vector3 Lead_Offset = Vector3.Normalize(Leader.transform.position);
        Final_Move = Final_Move + Lead_Offset;

        return Final_Move;

    }
}
        
    

