using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicRangedBehaviour : TreeActions
{
    
    
    public GameObject PatrolPoint1;
    public GameObject PatrolPoint2;
    public GameObject PatrolPoint3;

    TreeNodes.Status mTreeStatus = TreeNodes.Status.RUNNING;
    private void Awake()
    {
        AI = GetComponent<RangeAI>();
        PatrolPoints.Add(PatrolPoint1);
        PatrolPoints.Add(PatrolPoint2);
        PatrolPoints.Add(PatrolPoint3);
        CurrentPatrolPoint = PatrolPoints.Count - 1;
    }
    // Start is called before the first frame update
    void Start()
    {

        //make the root node of the entity behaviour tree.
        mRoot = new TreeRoot();
        //Sequence node setup with a condition leaf for refence.
        TreeSelector PatrolSelector = new TreeSelector("Patrol Selector");
        TreeLeaf FleeCondition = new TreeLeaf("Flee Condition", f_FleeCondition);
        TreeSequence VisionCheck = new TreeSequence("Vision Check");
        TreeLeaf CanSeeEnemies = new TreeLeaf("Can See Enemies", f_CanSeeEnemies);
        TreeSelector AttackSelector = new TreeSelector("Attack Selector");
        TreeSequence RangeCheck = new TreeSequence("Range Check");
        TreeLeaf InRangeOfEnemies = new TreeLeaf("In Range Of Enemies", f_InRangeOfEnemies);
        TreeLeaf AttackEnemies = new TreeLeaf("Attack Enemies", f_AttackEnemies);
        TreeSequence MoveToTargetAndAttack = new TreeSequence("Move To Target And Attack");
        TreeLeaf FindTarget = new TreeLeaf("Find Target", f_FindTarget);
        TreeLeaf MoveToTarget = new TreeLeaf("Move To Target", f_MoveToTarget);
        TreeSequence Patrol = new TreeSequence("Patrol");
        TreeLeaf FindNextPoint = new TreeLeaf("Find Next Point", f_FindNextPoint);
        TreeLeaf MoveToNextPoint = new TreeLeaf("Move To Next Point", f_MoveToNextPoint);

        //adding leafs to sequence as children including a condition one then adding the sequece to the root.
        PatrolSelector.AddChild(FleeCondition);
        PatrolSelector.AddChild(VisionCheck);
        VisionCheck.AddChild(CanSeeEnemies);
        VisionCheck.AddChild(AttackSelector);
        AttackSelector.AddChild(RangeCheck);
        RangeCheck.AddChild(InRangeOfEnemies);
        RangeCheck.AddChild(AttackEnemies);
        AttackSelector.AddChild(MoveToTargetAndAttack);
        MoveToTargetAndAttack.AddChild(FindTarget);
        MoveToTargetAndAttack.AddChild(MoveToTarget);
        MoveToTargetAndAttack.AddChild(AttackEnemies);
        PatrolSelector.AddChild(Patrol);
        Patrol.AddChild(FindNextPoint);
        Patrol.AddChild(MoveToNextPoint);
        mRoot.AddChild(PatrolSelector);


        mRoot.PrintTree();
        
    }

}
