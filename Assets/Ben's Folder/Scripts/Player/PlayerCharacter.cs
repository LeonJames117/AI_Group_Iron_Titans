using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
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

    AudioSource audioSource;
    [SerializeField] AudioClip ouch_AudioClip;
    [SerializeField] GameObject blood;

    [SerializeField] BoxCollider attackBox;
    public bool attacking = false;

    [SerializeField] Canvas deathCanvas;

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
        audioSource = GetComponent<AudioSource>();
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
    }

    public void Damage(int damage) 
    {
        health -= damage;
        audioSource.PlayOneShot(ouch_AudioClip);
        
        if(health <= 0) 
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            deathCanvas.gameObject.SetActive(true);
        }
    }
}
