using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking_Agent : MonoBehaviour
{
    [SerializeField] public Vector3 Velocity;
    [SerializeField] public string Flock_Type;
    public int Neighbor_Num;

    public Vector3 Get_Location()
    {
        return transform.position;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
