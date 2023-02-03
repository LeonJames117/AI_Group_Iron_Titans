using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Controller : MonoBehaviour
{

    [SerializeField] public Flocking_Agent[]  Flock_Agents = new Flocking_Agent[10];
    Vector3 New_Vect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Flocking_Agent Target_Agent = Flock_Agents[0];

        for (int i = 0; i < Flock_Agents.Length; i++)
        {
            if(Flock_Agents[i] != Target_Agent)
            {
                
                if (Vector3.Distance(Flock_Agents[i].Get_Location(),Target_Agent.Get_Location()) > 200)
                {
                    New_Vect = Flock_Agents[i].transform.position += Target_Agent.Get_Location();
                    Flock_Agents[i].Neighbor_Num++;
                }
                New_Vect /= Flock_Agents[i].Neighbor_Num;
                New_Vect.Normalize();
            }

        }
    }
}
