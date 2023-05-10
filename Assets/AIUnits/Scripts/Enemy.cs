using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Vector3 bodyPos;
    public LayerMask TargetLayer;
    public LayerMask Obstructions;
    public bool PlayerInSight;
    public float VisRadius = 10f;
    [Range(0, 360)]
    public float VisAngle;
    List<Vector3> Path = new List<Vector3>();

    public Animator Attack_Anim;

    public AudioSource audioSource;
    public AudioClip hit_audioClip;
    public GameObject blood;
    public Wave wave;
    public CameraShake cameraShake;
    public GameObject Arrow;
    public Transform Fire_Point;

    bool isStopped = true;
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

    private void Update()
    {
        bodyPos = Body.transform.position;
    }

    private void FOV_Check()
    {
        Collider[] Vision = Physics.OverlapSphere(Body.transform.position, VisRadius, TargetLayer);
        if (Vision.Length != 0)
        {
            
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
                //can see
                RaycastHit hit;
                print("Passed if 1");
                float Distance_to_Target = Vector3.Distance(transform.position, Target_Pos);

                if (true)
                {
                    if (isStopped) 
                    {
                        LookArround = true;
                    }
                    else 
                    {
                        LookArround = false;
                    }

                    print("Passed if 2 " + this.name);
  
                    PlayerInSight = true;
                    print("PlayerInSight " + PlayerInSight);
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
                LookArround = false;
                PlayerInSight = false;
            }

        }
        else if (PlayerInSight)
        {
            //LookArround = true;
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
        isStopped = true;
        NavAgent.CompletePath();
    }

    public void SetUpMovement(Vector3 endPoint)
    {
        Vector3 BodyNoY = new Vector3(Body.transform.position.x, 0, Body.transform.position.z);
         Path = PathfindRequestManager.instance.RequestPath(BodyNoY, endPoint);
        if (Path == null)
        {
            print("Path is Null");
            return;
        }
        if (Path.Count == 0)
        {
            print("Path count 0");
            return;
        }
        int Index = Path.Count - 1;
        LastPathPointPos = Path[Index];
    }

    public void Movement() 
    {
        isStopped = false;
        print("Currently Moving");
        NavAgent.StartFollowPath(Path);
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
            
            if(Type != "Flocking")
            {
                print("Playing Animation");
                Attack_Anim.SetBool("isAttacking", true);
            }
            if (Type == "Range")
            {
                StartCoroutine(RangeAttack());
                EndOfCooldown = Time.time + AttackCooldown;
                return TreeNodes.Status.SUCCESS;
            }
            EndOfCooldown = Time.time + AttackCooldown;
            print("Attack Successful");
            player.Damage(AttackDamge);
            return TreeNodes.Status.SUCCESS;
        }
        else 
        {
            if (Type != "Flocking")
                Attack_Anim.SetBool("isAttacking", false);
        }
        
        print("Attack on cooldown " + EndOfCooldown);
        print("Time " + Time.time);
        return TreeNodes.Status.FAILURE;   
    }

    IEnumerator RangeAttack() 
    {
        yield return new WaitForSeconds(0.48f);
        var temp = Instantiate(Arrow, Fire_Point.position, Fire_Point.rotation);
        temp.GetComponent<Projectile>().Damage = AttackDamge;
    }

    protected void Heal(int amount)
    {
        Health += amount;
    }

    public void Damage(int damage)
    {
        Health -= damage;
        audioSource.PlayOneShot(hit_audioClip, 0.3f);
        cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.4f));
        if (Health <= 0)
        {
            Instantiate(blood, Body.transform.position, Quaternion.identity);
            wave.UnitDeath();
            Destroy(gameObject);
        }
    }

    protected void RangeSetUp()
    {
        NavAgent = GetComponentInChildren<Nav_Agent>();
        MaxHealth = Health;
        Type = "Range";
    }

    protected void MeleeSetUp()
    {
        NavAgent = GetComponentInChildren<Nav_Agent>();
        MaxHealth = Health;
        Type = "Melee";
    }

    protected void FloackingSetUp()
    {
        NavAgent = GetComponentInChildren<Nav_Agent>();
        MaxHealth = Health;
        Type = "Flocking";
    }

    

}
