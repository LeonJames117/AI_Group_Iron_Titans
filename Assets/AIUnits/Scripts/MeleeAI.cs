using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : Enemy
{
    BasicMeleeBehaviour BehaviourTree;

    private void Awake()
    {
        MeleeSetUp();
        BehaviourTree = GetComponent<BasicMeleeBehaviour>();
    }

    public override void LoopUpdate()
    {
        
    }
}
