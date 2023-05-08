using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    RoundSystem roundSystem;
    int units;
    int units_alive;
    bool defeated = false;

    // Start is called before the first frame update
    void Start()
    {
        roundSystem = FindObjectOfType<RoundSystem>();
        units = transform.childCount;
        units_alive = units;
    }

    public void UnitDeath() 
    {
        units_alive--;

        if(units_alive == 0) 
        {
            WaveComplete();
            defeated = true;
        }
    }

    void WaveComplete() 
    {
        roundSystem.WaveComplete();
    }
}
