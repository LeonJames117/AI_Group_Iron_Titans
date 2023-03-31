using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessControls();
    }

    void ProcessControls() 
    {
        KeyboardProcess();
        MouseProcess();
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
            character.Attack();
        }

        character.SetMoveDirection(dirVector);
        if (dirVector != Vector3.zero)//Set Move State
        {
            character.SetMoveState(Character.MoveState.WALKING);
        }
        else 
        {
            character.SetMoveState(Character.MoveState.IDLE);
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
