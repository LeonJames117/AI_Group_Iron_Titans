using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    RoundSystem roundSystem;
    int units;
    int units_alive;
    bool defeated = false;
    [SerializeField] GameObject[] unit_arr;


    // Start is called before the first frame update
    void Start()
    {
        roundSystem = FindObjectOfType<RoundSystem>();
        units = unit_arr.Length;

        units_alive = units;
    }

    private void Update()
    {


        print("child count: " + units_alive);
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
