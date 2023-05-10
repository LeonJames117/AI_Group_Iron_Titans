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
    public float WaitAtPoint = 5f;
    float MoveToNextPoint;
    //Condition leaf.

    private void Start()
    {
        

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
        print("Attacking");
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
        print("Attack failed");
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
        if(AI.PlayerInSight)
        {
            return Movement(Player.transform.position);
        }
        return TreeNodes.Status.FAILURE;
    }

    //the Actions leafs.
    public TreeNodes.Status f_FindNextPoint()
    {
        if (Time.time < MoveToNextPoint)
        {
            //AI.LookArround = true;
            return TreeNodes.Status.FAILURE;
        }
        if (CurrentPatrolPoint == PatrolPoints.Count - 1)
        {
            CurrentPatrolPoint = 0;
            MoveToNextPoint = Time.time + WaitAtPoint;
            AI.LookArround = false;
        }
        else
        {
            CurrentPatrolPoint++;
            MoveToNextPoint = Time.time + WaitAtPoint;
            AI.LookArround = false;
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
        AI.SetUpMovement(destination);
        //if (mState == ActionState.IDLE)
        //{
        //    mState = ActionState.WORKING;
        //}
        if (Vector3.Distance(AI.GetLastPointPos(), destination) >= AI.AttackRange)
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

        AI.Movement();
        return TreeNodes.Status.RUNNING;
    }

    TreeNodes.Status PatrolMovement(Vector3 destination)
    {
        float DistanceToTarget = Vector3.Distance(destination, AI.Body.transform.position);
        AI.SetUpMovement(destination);
        //if (mState == ActionState.IDLE)
        //{
        //    mState = ActionState.WORKING;
        //}
        if(AI.PlayerInSight)
        {
            mState = ActionState.IDLE;
            AI.LookArround = false;
            AI.StopMovement();
            return TreeNodes.Status.SUCCESS;
        }
        else if (DistanceToTarget < 4)
        {
            //AI.LookArround = true;
            AI.StopMovement();
            mState = ActionState.IDLE;
            return TreeNodes.Status.SUCCESS;
        }

        AI.Movement();
        return TreeNodes.Status.RUNNING;
    }

    public void TreeUpdate()
    {
        mRoot.Process();
    }


}
