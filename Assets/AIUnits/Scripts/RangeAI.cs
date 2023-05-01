using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAI : Enemy
{

    private void Awake()
    {
        RangeSetUp();
        BehaviourTree = GetComponent<BasicRangedBehaviour>();
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
