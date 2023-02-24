using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
  
    [SerializeField] public float lookSpeed;
    [SerializeField] public float health;

    Vector3 direction = Vector3.zero;

    enum MoveState
    {
        IDLE,
        WALKING,
        RUNNING
    }

    MoveState moveState = MoveState.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        switch (moveState)
        {
            case MoveState.IDLE:
                break;
            case MoveState.WALKING:
                break;
            case MoveState.RUNNING:
                break;
            default:
                break;
        }
    }

    void SetMoveDirection(Vector3 p_direction) 
    {
        direction = Vector3.Normalize(p_direction);
    }

    void ProcessIdle() 
    {

    }

    void ProcessWalking() 
    {

    }

    void ProcessRunning() 
    {

    } 

    void Attack()
    {

    }
}
