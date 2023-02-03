using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Character character;
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
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.position += this.transform.forward * character.moveSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.position -= this.transform.forward * character.moveSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.position -= this.transform.right * character.moveSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.position += this.transform.right * character.moveSpeed * Time.deltaTime;
            }
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
            //hit.point
            float angleOfRotation = Vector3.Angle(Vector3.forward, hit.point - transform.position);

            Vector3 rotation = new Vector3(0, angleOfRotation, 0);

            print(rotation);

            transform.Rotate(rotation);
        }

    }
}
