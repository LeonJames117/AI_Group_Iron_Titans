using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flocking_Agent : MonoBehaviour
{
    Collider Agent_Collider;
    public FlockingAI AI;
    int Health = 20;
    public Flocking_Controller controller;
    public Collider Agent_Collider_Access { get { return Agent_Collider; } }
    // Start is called before the first frame update
    void Start()
    {
        Agent_Collider = GetComponent<Collider>();

    }

    public void Damage(int Dam)
    {
        Health -= Dam;
        if(Health <= 0)
        {
            foreach(Flocking_Agent Agent in controller.All_Agents)
            {
                if(Agent == this)
                {
                    controller.All_Agents.Remove(Agent);
                    controller.CheckDead();
                    Transform t = new GameObject().transform;
                    t.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Instantiate(AI.blood, t.position, Quaternion.identity);
                    AI.cameraShake.StartCoroutine(AI.cameraShake.Shake(0.1f, 0.4f));
                    AI.audioSource.PlayOneShot(AI.hit_audioClip, 0.3f);
                    AI.wave.UnitDeath();
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackBox")
        {
            Damage(20);
            
        }
    }

    public void Move_To(Vector3 Destination)
    {
        //Face new destination
        transform.forward = Destination;
        //Movement
        float NewX = transform.position.x;
        float NewZ = transform.position.z;
        NewX+= Destination.x * Time.deltaTime;
        NewZ += Destination.z * Time.deltaTime;
        transform.position = new Vector3(NewX,0.86f, NewZ);
    }

    public bool Range_Check(Vector3 Target, float Range)
    {
        return Vector3.Distance(Target, transform.position) <= Range;
    }
    
    
    
    
    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
