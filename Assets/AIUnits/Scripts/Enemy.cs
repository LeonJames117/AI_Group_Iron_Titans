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
    public bool LookArround = true;
    float EndOfCooldown;
    protected Nav_Agent NavAgent;
    Vector3 LastPathPointPos;
    protected TreeActions BehaviourTree;
    public string Type = "";
    public GameObject Target;
    public GameObject Body;
    public LayerMask TargetLayer;
    public LayerMask Obstructions;
    public bool PlayerInSight;
    public float VisRadius = 10f;
    [Range(0, 360)]
    public float VisAngle;


    

    public AudioSource audioSource;
    public AudioClip hit_audioClip;
    public GameObject blood;
    public Wave wave;
    public CameraShake cameraShake;

    void Start()
    {
        Target = GameObject.FindObjectOfType<PlayerCharacter>().gameObject;
        cameraShake = FindObjectOfType<CameraShake>();
        wave = GetComponentInParent<Wave>();
        StartCoroutine(FOV_Run());
    }

    private IEnumerator FOV_Run()
    {
        float Wait_Time = 0.05f;
        WaitForSeconds Delay = new WaitForSeconds(Wait_Time);

        while (true)
        {
            FOV_Check();
            yield return Delay;

        }
    }


    private void FOV_Check()
    {
        Collider[] Vision = Physics.OverlapSphere(Body.transform.position, VisRadius, TargetLayer);
        if (Vision.Length != 0)
        {
            print("PlayerInSight");
            Vector3 Target_Pos = Vision[0].transform.position;
            Vector3 TargetNoY = new Vector3(Target_Pos.x, 0, Target_Pos.z);
            Vector3 BodyNoY = new Vector3(Body.transform.position.x, 0, Body.transform.position.z);
            Vector3 Target_Direction = (TargetNoY - BodyNoY).normalized;
            
            if(LookArround)
            {
                Quaternion Look_At = Quaternion.LookRotation(TargetNoY - BodyNoY);

                Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, Look_At, 50 * Time.deltaTime);
            }
            
            
            if (Vector3.Angle(Body.transform.forward, Target_Direction) < VisAngle / 2)
            {
                RaycastHit hit;
                print("Passed if 1");
                float Distance_to_Target = Vector3.Distance(transform.position, Target_Pos);
                if (!Physics.Raycast(transform.position, Target_Direction, out hit, Distance_to_Target, Obstructions))
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
        print("Stopped");
        NavAgent.CompletePath();
    }



    public void Movement(Vector3 endPoint) 
    {
        Vector3 BodyNoY = new Vector3(Body.transform.position.x, 0, Body.transform.position.z);
        List<Vector3> path = PathfindRequestManager.instance.RequestPath(BodyNoY, endPoint);
        if (path == null)
        {
            print("Path is Null");
            return;
        }
        if(path.Count == 0)
        {
            print("Path count 0");
            return;
        }
        int Index = path.Count - 1;
        LastPathPointPos = path[Index];
        print("Currently Moving");
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

    public TreeNodes.Status Attack(PlayerCharacter player)
    {
        if (Time.time >= EndOfCooldown)
        {
            player.Damage(AttackDamge);
            EndOfCooldown = Time.time + AttackCooldown;
            print("Attack Successful");
            return TreeNodes.Status.SUCCESS;
        }
        print("Attack on cooldown " + EndOfCooldown);
        print("Time " + Time.time);
        return TreeNodes.Status.FAILURE;   
    }

    protected void Heal(int amount)
    {
        Health += amount;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Instantiate(blood, Body.transform.position, Quaternion.identity);
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.4f));
            audioSource.PlayOneShot(hit_audioClip, 0.3f);
            wave.UnitDeath();
            Destroy(gameObject);
        }
    }

    protected void RangeSetUp()
    {
        NavAgent = GetComponentInChildren<Nav_Agent>();
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
        NavAgent = GetComponentInChildren<Nav_Agent>();
        Health = 20;
        MaxHealth = Health;
        AttackRange = 5f;
        AttackCooldown = 30f;
        AttackDamge = 20;
        VisRadius = 45;
        VisAngle = 60;
    }

    protected void FloackingSetUp()
    {
        NavAgent = GetComponentInChildren<Nav_Agent>();
        Health = 100;
        MaxHealth = Health;
        AttackRange = 1f;
        AttackCooldown = 10f;
        AttackDamge = 5;
        VisRadius = 50;
        VisAngle = 50;
        Type = "Flocking";
    }

    

}
