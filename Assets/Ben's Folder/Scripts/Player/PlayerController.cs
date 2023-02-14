using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Character character;
    [SerializeField] GameObject characterBody;

    [SerializeField] bool usePhysics = false;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();

        rigidbody = GetComponentInChildren<Rigidbody>();
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
        if (!usePhysics)
        {
            Vector3 dirVector = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                dirVector += this.transform.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dirVector -= this.transform.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                dirVector -= this.transform.right;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dirVector += this.transform.right;
            }

            dirVector = Vector3.Normalize(dirVector);

            gameObject.transform.position += dirVector * character.moveSpeed * Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                rigidbody.AddForce(this.transform.forward * character.moveSpeed * Time.deltaTime * 1000);
                print("forward");

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                rigidbody.AddForce(this.transform.forward * character.moveSpeed * -1 * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                rigidbody.AddForce(this.transform.right * character.moveSpeed * -1 * Time.deltaTime);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                rigidbody.AddForce(this.transform.right * character.moveSpeed * Time.deltaTime);
            }
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

            characterBody.transform.forward = Vector3.Normalize(aimVector);
        }
    }
}
