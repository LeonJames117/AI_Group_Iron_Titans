using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockingBehaviour : MonoBehaviour
{
    
    bool ConditionFulfilled = true;
    TreeRoot mRoot;
    public GameObject Player;
    FlockingAI AI;
    public enum ActionState { IDLE, WORKING};
    ActionState mState = ActionState.IDLE;

    TreeNodes.Status mTreeStatus = TreeNodes.Status.RUNNING;

    private void Awake()
    {
        AI = GetComponent<FlockingAI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //uncomment when when nave mesh is attached to model.
        //mAgent = this.GetComponent<NavMeshAgent>();

        //make the root node of the entity behaviour tree.
        mRoot = new TreeRoot();
        //Sequence node setup with a condition leaf for refence.
        TreeSelector RegroupSelector = new TreeSelector("Regroup Selector");
        TreeLeaf RegroupCondition = new TreeLeaf("Regroup Condition", f_RegroupCondition);
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
        RangeCheck.AddChild(InRangeOfEnemies);
        RangeCheck.AddChild(AttackEnemies);
        AttackSelector.AddChild(RangeCheck);
        MoveToTargetAndAttack.AddChild(FindTarget);
        MoveToTargetAndAttack.AddChild(MoveToTarget);
        MoveToTargetAndAttack.AddChild(AttackEnemies);
        AttackSelector.AddChild(MoveToTargetAndAttack);
        VisionCheck.AddChild(CanSeeEnemies);
        VisionCheck.AddChild(AttackSelector);
        RegroupSelector.AddChild(RegroupCondition);
        RegroupSelector.AddChild(VisionCheck);
        mRoot.AddChild(RegroupSelector);


        mRoot.PrintTree();

    }

    //Condition leaf.
    public TreeNodes.Status f_RegroupCondition()
    {
        if (AI.Health <= AI.Health * 0.25) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    public TreeNodes.Status f_CanSeeEnemies()
    {
        if (ConditionFulfilled) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    public TreeNodes.Status f_InRangeOfEnemies()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= AI.AttackRange) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    //the Actions leafs.
    public TreeNodes.Status f_AttackEnemies()
    {
        AI.Attack();
        return TreeNodes.Status.SUCCESS;
    }

    public TreeNodes.Status f_FindTarget()
    {
        //put what the function is meant to do here.
        return TreeNodes.Status.SUCCESS;
    }

    public TreeNodes.Status f_MoveToTarget()
    {
        return Movement(Player.transform.position);
    }

    //Movement action to be called by action leafs.
    TreeNodes.Status Movement(Vector3 destination)
    {
        float DistanceToTarget = Vector3.Distance(destination, this.transform.position);
        if(mState == ActionState.IDLE)
        {
            AI.Movement(destination);
            mState = ActionState.WORKING;
        }
        else if(Vector3.Distance(AI.GetLastPointPos(), destination) >= 2)
        {
            mState = ActionState.IDLE;
            return TreeNodes.Status.FAILURE;
        }
        else if(DistanceToTarget < 2)
        {
            mState = ActionState.IDLE;
            return TreeNodes.Status.SUCCESS;
        }
        return TreeNodes.Status.RUNNING;
    }


    // Update is called once per frame
    void Update()
    {
        //will run tree if the tree status is no success full will need to adapted this so it is the right one for the right entity behavior.
        if(mTreeStatus != TreeNodes.Status.SUCCESS)
        {
            mTreeStatus = mRoot.Process();
        }
    }
}
