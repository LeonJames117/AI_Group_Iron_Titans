using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeActions : MonoBehaviour
{
    protected TreeRoot mRoot;
    protected bool ConditionFulfilled = false;
    protected Enemy AI;
    public Character Player;
    public enum ActionState { IDLE, WORKING };
    protected ActionState mState = ActionState.IDLE;
    protected List<GameObject> PatrolPoints = new List<GameObject>();
    protected int CurrentPatrolPoint = 2;


    //Condition leaf.

    public TreeNodes.Status f_FleeCondition()
    {
        if (AI.Health <= AI.MaxHealth * 0.25) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    public TreeNodes.Status f_RegroupCondition()
    {
        if (AI.Health <= AI.MaxHealth * 0.25) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    //Waiting on vision to get added to the AI.
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
        return AI.Attack(Player);
    }

    //Prob not need may remove.
    public TreeNodes.Status f_FindTarget()
    {
        return TreeNodes.Status.SUCCESS;
    }

    public TreeNodes.Status f_MoveToTarget()
    {
        return Movement(Player.transform.position);
    }

    //the Actions leafs.
    public TreeNodes.Status f_FindNextPoint()
    {
        if(CurrentPatrolPoint == PatrolPoints.Count - 1)
        {
            CurrentPatrolPoint = 0;
        }
        else
        {
            CurrentPatrolPoint++;
        }
        return TreeNodes.Status.SUCCESS;
    }

    public TreeNodes.Status f_MoveToNextPoint()
    {
        return PatrolMovement(PatrolPoints[CurrentPatrolPoint].transform.position);
    }

    //Movement action to be called by action leafs.
    TreeNodes.Status Movement(Vector3 destination)
    {
        float DistanceToTarget = Vector3.Distance(destination, this.transform.position);
        AI.Movement(destination);
        if (mState == ActionState.IDLE)
        {
            mState = ActionState.WORKING;
        }
        else if (Vector3.Distance(AI.GetLastPointPos(), destination) >= AI.AttackRange)
        {
            mState = ActionState.IDLE;
            return TreeNodes.Status.FAILURE;
        }
        else if (DistanceToTarget < AI.AttackRange)
        {
            mState = ActionState.IDLE;
            return TreeNodes.Status.SUCCESS;
        }
        return TreeNodes.Status.RUNNING;
    }

    TreeNodes.Status PatrolMovement(Vector3 destination)
    {
        float DistanceToTarget = Vector3.Distance(destination, this.transform.position);
        AI.Movement(destination);
        if (mState == ActionState.IDLE)
        {
            mState = ActionState.WORKING;
        }
        else if (DistanceToTarget < 2)
        {
            mState = ActionState.IDLE;
            return TreeNodes.Status.SUCCESS;
        }
        return TreeNodes.Status.RUNNING;
    }

    public void TreeUpdate()
    {
        mRoot.Process();
    }


}
