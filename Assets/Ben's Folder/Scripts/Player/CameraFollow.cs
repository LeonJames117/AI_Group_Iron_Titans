using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject followObject;

    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = followObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaVector = followObject.transform.position - lastPos;
        this.transform.position += deltaVector;

        lastPos = followObject.transform.position;
    }
}
