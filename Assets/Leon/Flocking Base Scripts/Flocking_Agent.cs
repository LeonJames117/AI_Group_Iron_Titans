using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flocking_Agent : MonoBehaviour
{
    Collider Agent_Collider;
    public Collider Agent_Collider_Access { get { return Agent_Collider; } }
    // Start is called before the first frame update
    void Start()
    {
        Agent_Collider = GetComponent<Collider>();

    }

    public void Move_To(Vector3 Destination)
    {
        //Face new destination
        transform.forward = Destination;
        //Movement
        transform.position += Destination * Time.deltaTime;
    }
   
    
    
    
    
    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
