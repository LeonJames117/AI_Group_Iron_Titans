using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int MaxHealth;
    public float AttackRange;
    public float AttackCooldown;
    public int AttackDamge;
    protected Nav_Agent NavAgent;
    Vector3 LastPathPointPos;
    protected TreeActions BehaviourTree;


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
        player.health -= AttackDamge;
        return TreeNodes.Status.SUCCESS;
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
    }

    protected void MeleeSetUp()
    {
        NavAgent = GetComponent<Nav_Agent>();
        Health = 250;
        MaxHealth = Health;
        AttackRange = 5f;
        AttackCooldown = 30f;
        AttackDamge = 20;
    }

    protected void FloackingSetUp()
    {
        NavAgent = GetComponent<Nav_Agent>();
        Health = 100;
        MaxHealth = Health;
        AttackRange = 5f;
        AttackCooldown = 10f;
        AttackDamge = 5;
    }

}
