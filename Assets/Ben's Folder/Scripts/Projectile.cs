using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    bool fired = true;

    public void Fire(Quaternion rotation) 
    {
        fired = true;
        this.transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        if (fired) 
        {
            transform.position += this.transform.forward * speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            //damagePlayer;

        }
        print("collide");
        Destroy(gameObject);
    }
}
