using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damge : MonoBehaviour
{
    public Enemy AI;
    private void OnTriggerEnter(Collider other)
    {
        print("Overlap");
        if (other.tag == "AttackBox")
        {
            print("Damage Triggered");
            AI.Damage(20);

        }
    }
}
