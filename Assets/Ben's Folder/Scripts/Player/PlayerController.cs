using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerCharacter character;
    [SerializeField] Animator body_animator;
    [SerializeField] Animator sword_animator;
    bool hasMoved = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip whoosh_audioClip;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessControls();
    }



    void ProcessControls() 
    {
        hasMoved = false;
        KeyboardProcess();
        MouseProcess();

        if (hasMoved)
        {
            body_animator.SetBool("isMoving", true);
            sword_animator.SetBool("isMoving", true);
        }
        else
        {
            body_animator.SetBool("isMoving", false);
            sword_animator.SetBool("isMoving", false);
        }
    }

    void KeyboardProcess() 
    {
        Vector3 dirVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))//Forward
        {
            dirVector += this.transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))//Backward
        {
            dirVector -= this.transform.forward;
        }

        if (Input.GetKey(KeyCode.A))//Left
        {
            dirVector -= this.transform.right;
        }
        else if (Input.GetKey(KeyCode.D))//Right
        {
            dirVector += this.transform.right;
        };

        if (Input.GetMouseButtonDown(0)) 
        {
            if (!character.attacking)
            {
                character.Attack();
                audioSource.PlayOneShot(whoosh_audioClip, 0.05f);
                sword_animator.SetBool("isAttacking", true);
            }
        }
        else 
        {
            sword_animator.SetBool("isAttacking", false);
        }

        character.SetMoveDirection(dirVector);
        if (dirVector != Vector3.zero)//Set Move State
        {
            character.SetMoveState(PlayerCharacter.MoveState.WALKING);
            hasMoved = true;
        }
        else 
        {
            character.SetMoveState(PlayerCharacter.MoveState.IDLE);
        }
    }

    void MouseProcess()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if(Physics.Raycast(castPoint, out hit, Mathf.Infinity)) 
        {
            Vector3 aimVector = hit.point - transform.position;
            aimVector.y = 0;

            character.SetAimDirection(aimVector);
        }
    }
}
