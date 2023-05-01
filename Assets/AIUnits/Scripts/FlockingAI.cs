using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : Enemy
{
    private void Awake()
    {
        FloackingSetUp();
        BehaviourTree = GetComponent<FlockingBehaviour>();
    }

    public override void LoopUpdate()
    {
        base.LoopUpdate();
    }

    private void Update()
    {
        LoopUpdate();
    }
}
