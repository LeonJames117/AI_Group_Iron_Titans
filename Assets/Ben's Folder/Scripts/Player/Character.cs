using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
 
    [SerializeField] public float health;

    Vector3 moveDirection = Vector3.zero;
    Vector3 aimDirection = Vector3.zero;

    [SerializeField] GameObject characterBody;

    [SerializeField] float attackCooldown = 0.5f;
    float attackTimer = 0.0f;
    [SerializeField] float attackWindow = 0.5f;
    float attackWindowTimer = 0.0f;


    [SerializeField] BoxCollider attackBox;
    public bool attacking = false;

    public enum MoveState
    {
        IDLE,
        WALKING,
        RUNNING
    }

    MoveState moveState = MoveState.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        attackBox.gameObject.SetActive(false);
        //attackBox.enabled = false;
    }

    private void Update()
    {
        ProcessTimers();
        ProcessAim();
        ProcessAttacking();

        switch (moveState)
        {
            case MoveState.IDLE:
                ProcessIdle();
                break;
            case MoveState.WALKING:
                ProcessWalking();
                break;
            case MoveState.RUNNING:
                ProcessRunning();
                break;
            default:
                break;
        }
    }

    public void ProcessAttacking() 
    {
        if (attacking) 
        {
            attackTimer += Time.deltaTime;

            if(attackWindow < attackTimer) 
            {
                attackBox.gameObject.SetActive(false);
            }

            if(attackCooldown < attackTimer) 
            {
                attacking = false;
                attackTimer = 0.0f;
            }
        }

        //if(attackWindowTimer >= attackWindow) 
        //{
        //    attackBox.enabled = false;
        //    attackWindowTimer = 0.0f;
        //}
    }

    public void ProcessTimers() 
    {
        attackWindowTimer += Time.deltaTime;
    }

    public void SetMoveState(MoveState p_moveState) 
    {
        moveState = p_moveState;
    }

    public void SetMoveDirection(Vector3 p_direction) 
    {
        moveDirection = Vector3.Normalize(p_direction);
    }

    public void SetAimDirection(Vector3 p_direction) 
    {
        aimDirection = p_direction;
    }

    void ProcessIdle() 
    {

    }

    void ProcessWalking() 
    {
        gameObject.transform.position += moveDirection * walkSpeed * Time.deltaTime;
    }

    void ProcessRunning() 
    {
        gameObject.transform.position += moveDirection * runSpeed * Time.deltaTime;
    } 

    void ProcessAim() 
    {
        if(aimDirection != Vector3.zero) 
        {
            characterBody.transform.forward = Vector3.Normalize(aimDirection);
        }
    }

    public void Attack()
    {
        if (!attacking) 
        {
            attacking = true;
            attackBox.gameObject.SetActive(true);
        }



        //if(attackTimer >= attackCooldown) 
        //{
        //    attackBox.gameObject.SetActive(true);
        //    //attackBox.enabled = true;
        //    print("attack");
        //    attackTimer = 0;
        //    attackWindowTimer = 0.0f;
        //}
    }
}
