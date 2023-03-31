using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAI : Enemy
{
    BasicRangedBehaviour BehaviourTree;

    private void Awake()
    {
        RangeSetUp();
        BehaviourTree = GetComponent<BasicRangedBehaviour>();
    }

    public override void LoopUpdate()
    {
        
    }
}
