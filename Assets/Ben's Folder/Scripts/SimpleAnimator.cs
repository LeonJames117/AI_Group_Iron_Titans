using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    [SerializeField] GameObject entityBody;

    [SerializeField] bool sideToSideRotation = false;
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (sideToSideRotation) 
        {
            //transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(timer) * 60.0f);
            //print(transform.rotation);
        }
    }

    void ProcessSideToSideRotation() 
    {

    }
}
