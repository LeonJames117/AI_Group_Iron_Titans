using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int MaxHealth;
    public float AttackRange;
    public float AttackCooldown = 5f;
    public int AttackDamge;
    float EndOfCooldown;
    protected Nav_Agent NavAgent;
    Vector3 LastPathPointPos;
    protected TreeActions BehaviourTree;
    public GameObject Target;
    public GameObject Body;
    public LayerMask TargetLayer;
    public LayerMask Obstructions;
    public bool PlayerInSight;
    public float VisRadius = 10f;
    [Range(0, 360)]
    public float VisAngle;


    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOV_Run());
    }

    private IEnumerator FOV_Run()
    {
        float Wait_Time = 0.5f;
        WaitForSeconds Delay = new WaitForSeconds(Wait_Time);

        while (true)
        {
            FOV_Check();
            yield return Delay;

        }
    }


    private void FOV_Check()
    {
        Collider[] Vision = Physics.OverlapSphere(transform.position, VisRadius, TargetLayer);
        if (Vision.Length != 0)
        {
            print("PlayerInSight");
            Vector3 Target_Pos = Vision[0].transform.position;
            Vector3 Target_Direction = (Vision[0].transform.position - transform.position).normalized;
            if (Vector3.Angle(Body.transform.forward, Target_Direction) < VisAngle / 2)
            {
                print("Passed if 1");
                float Distance_to_Target = Vector3.Distance(transform.position, Target_Pos);
                if (!Physics.Raycast(transform.position, Target_Direction, Distance_to_Target, Obstructions))
                {
                    print("Passed if 2");
                    PlayerInSight = true;
                }
                else
                {
                    print("Failed if 2");
                    PlayerInSight = false;
                }
            }
            else
            {
                print("Failed if 1");
                PlayerInSight = false;
            }

        }
        else if (PlayerInSight)
        {
            PlayerInSight = false;
        }
        else
        {
            print("Player could not be found");
        }

    }

    public void StopMovement()
    {
        NavAgent.CompletePath();
    }



    public void Movement(Vector3 endPoint) 
    {
        List<Vector3> path = PathfindRequestManager.instance.RequestPath(transform.position, endPoint);
        int Index = path.Count - 1;
        LastPathPointPos = path[Index];
        NavAgent.StartFollowPath(path);
    }

    public Vector3 GetLastPointPos()
    {
        return LastPathPointPos;
    }

    public virtual void LoopUpdate() 
    {
        BehaviourTree.TreeUpdate();
    }

    public TreeNodes.Status Attack(Character player)
    {
        if (Time.time >= EndOfCooldown)
        {
            player.health -= AttackDamge;
            EndOfCooldown = Time.time + AttackCooldown;
            return TreeNodes.Status.SUCCESS;
        }
        
    }

    protected void Heal(int amount)
    {
        Health += amount;
    }

    public void Damage(int damage)
    {
        Health -= damage;
    }

    protected void RangeSetUp()
    {
        NavAgent = GetComponent<Nav_Agent>();
        Health = 150;
        MaxHealth = Health;
        AttackRange = 20f;
        AttackCooldown = 20f;
        AttackDamge = 15;
        VisRadius = 60;
        VisAngle = 90;
    }

    protected void MeleeSetUp()
    {
        NavAgent = GetComponent<Nav_Agent>();
        Health = 250;
        MaxHealth = Health;
        AttackRange = 5f;
        AttackCooldown = 30f;
        AttackDamge = 20;
        VisRadius = 45;
        VisAngle = 60;
    }

    protected void FloackingSetUp()
    {
        NavAgent = GetComponent<Nav_Agent>();
        Health = 100;
        MaxHealth = Health;
        AttackRange = 5f;
        AttackCooldown = 10f;
        AttackDamge = 5;
        VisRadius = 50;
        VisAngle = 50;
    }

}
