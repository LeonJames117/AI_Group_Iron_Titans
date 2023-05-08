using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeActions : MonoBehaviour
{
    protected TreeRoot mRoot;
    protected bool ConditionFulfilled = false;
    protected Enemy AI;
    public PlayerCharacter Player;
    public enum ActionState { IDLE, WORKING };
    protected ActionState mState = ActionState.IDLE;
    protected List<GameObject> PatrolPoints = new List<GameObject>();
    protected int CurrentPatrolPoint;
    public Flocking_Controller Flock;
    //Condition leaf.

    private void Start()
    {
        //Player = AI.Target.GetComponent<PlayerCharacter>();
        
    }

    public TreeNodes.Status f_FleeCondition()
    {
        if (AI.Health <= AI.MaxHealth * 0.25) return TreeNodes.Status.SUCCESS;
        return TreeNodes.Status.FAILURE;
    }

    //public TreeNodes.Status f_RegroupCondition()
    //{
    //    print("Tree Start");
    //    if (AI.Health <= AI.MaxHealth * 0.25) return TreeNodes.Status.SUCCESS;
    //    return TreeNodes.Status.FAILURE;
    //}

    //Waiting on vision to get added to the AI.
    public TreeNodes.Status f_CanSeeEnemies()
    {
        print("Checking Sight");
        if (AI.PlayerInSight) return TreeNodes.Status.SUCCESS;
        print("Failed to see");
        return TreeNodes.Status.FAILURE;
    }

    public TreeNodes.Status f_InRangeOfEnemies()
    {
        print("AI pos = " + AI.Body.transform.position);
        print("AI Attack Range = " + AI.AttackRange);
        print("Player pos = " + Player.transform.position);
        if(AI.Type == "Flocking")
        {
            foreach(Flocking_Agent Agent in Flock.All_Agents)
            {
                if (Agent.Range_Check(Player.transform.position,AI.AttackRange))
                {
                    print("FLock in range");
                    return TreeNodes.Status.SUCCESS;
                }
            }
            
        }
        else
        {
            if (Vector3.Distance(Player.transform.position, AI.Body.transform.position) <= AI.AttackRange) return TreeNodes.Status.SUCCESS;
            return TreeNodes.Status.FAILURE;
            
        }

        return TreeNodes.Status.FAILURE;
    }

    //the Actions leafs.
    public TreeNodes.Status f_AttackEnemies()
    {
        if (AI.Type == "Flocking")
        {
            foreach (Flocking_Agent Agent in Flock.All_Agents)
            {
                if (Agent.Range_Check(Player.transform.position, AI.AttackRange))
                {
                    return AI.Attack(Player);
                }
            }

        }
        else
        {
            return AI.Attack(Player);
        }
        return TreeNodes.Status.FAILURE;
    }

    //Prob not need may remove.
    public TreeNodes.Status f_FindTarget()
    {
        print("Found Target");
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
        float DistanceToTarget = Vector3.Distance(destination, AI.Body.transform.position);
        AI.Movement(destination);
        if (mState == ActionState.IDLE)
        {
            mState = ActionState.WORKING;
        }
        else if (Vector3.Distance(AI.GetLastPointPos(), destination) >= AI.AttackRange)
        {
            print("Cannot reach target");
            mState = ActionState.IDLE;
            AI.StopMovement();
            return TreeNodes.Status.FAILURE;
        }
        else if (DistanceToTarget < AI.AttackRange)
        {
            mState = ActionState.IDLE;
            AI.StopMovement();
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
        else if(AI.PlayerInSight)
        {
            mState = ActionState.IDLE;
            AI.StopMovement();
            return TreeNodes.Status.SUCCESS;
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
