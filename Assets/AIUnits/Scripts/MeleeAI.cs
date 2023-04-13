using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : Enemy
{

    private void Awake()
    {
        MeleeSetUp();
        BehaviourTree = GetComponent<BasicMeleeBehaviour>();
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
