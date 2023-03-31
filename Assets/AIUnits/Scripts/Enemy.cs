using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int Health = 100;
    protected int MaxHealth;
    protected float AttackRange;
    protected float AttackCooldown;
    protected int AttackDamge;
    protected Nav_Agent NavAgent;


    protected void Movement(Vector3 endPoint) 
    {
        NavAgent.StartFollowPath(PathfindRequestManager.instance.RequestPath(transform.position, endPoint));
    }

    public virtual void LoopUpdate() {}

    protected void Attack()
    {

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
