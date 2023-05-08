using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockingBehaviour : TreeActions
{
    

    TreeNodes.Status mTreeStatus = TreeNodes.Status.RUNNING;

    private void Awake()
    {
        AI = GetComponent<FlockingAI>();
        Player = GameObject.FindObjectOfType<PlayerCharacter>();
        if (Player == null)
        {
            Debug.LogError("Null Player");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //uncomment when when nave mesh is attached to model.
        //mAgent = this.GetComponent<NavMeshAgent>();

        //make the root node of the entity behaviour tree.
        mRoot = new TreeRoot();
        //Sequence node setup with a condition leaf for refence.
        TreeSequence RegroupSelector = new TreeSequence("Regroup Selector");
        //TreeLeaf RegroupCondition = new TreeLeaf("Regroup Condition", f_RegroupCondition);
        TreeSequence VisionCheck = new TreeSequence("Vision Check");
        TreeLeaf CanSeeEnemies = new TreeLeaf("Can See Enemies", f_CanSeeEnemies);
        TreeSelector AttackSelector = new TreeSelector("Attack Selector");
        TreeSequence RangeCheck = new TreeSequence("Range Check");
        TreeLeaf InRangeOfEnemies = new TreeLeaf("In Range Of Enemies", f_InRangeOfEnemies);
        TreeLeaf AttackEnemies = new TreeLeaf("Attack Enemies", f_AttackEnemies);
        TreeSequence MoveToTargetAndAttack = new TreeSequence("Move To Target And Attack");
        TreeLeaf FindTarget = new TreeLeaf("Find Target", f_FindTarget);
        TreeLeaf MoveToTarget = new TreeLeaf("Move To Target", f_MoveToTarget);

        //adding leafs to sequence as children including a condition one then adding the sequece to the root.
        //RegroupSelector.AddChild(RegroupCondition);
        RegroupSelector.AddChild(VisionCheck);
        VisionCheck.AddChild(CanSeeEnemies);
        VisionCheck.AddChild(AttackSelector);
        AttackSelector.AddChild(RangeCheck);
        RangeCheck.AddChild(InRangeOfEnemies);
        RangeCheck.AddChild(AttackEnemies);
        AttackSelector.AddChild(MoveToTargetAndAttack);
        MoveToTargetAndAttack.AddChild(FindTarget);
        MoveToTargetAndAttack.AddChild(MoveToTarget);
        MoveToTargetAndAttack.AddChild(AttackEnemies);
        mRoot.AddChild(RegroupSelector);


        mRoot.PrintTree();

    }

   
}
