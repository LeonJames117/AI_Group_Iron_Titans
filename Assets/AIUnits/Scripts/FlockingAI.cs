using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : Enemy
{
    FlockingBehaviour BehaviourTree;
    private void Awake()
    {
        FloackingSetUp();
        BehaviourTree = GetComponent<FlockingBehaviour>();
    }

    public override void LoopUpdate()
    {
        
    }
}
