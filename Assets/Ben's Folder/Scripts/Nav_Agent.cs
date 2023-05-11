using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_Agent : MonoBehaviour
{
    List<Vector3> waypoints;
    int waypoint_index = 0;
    bool followingPath = false;
    [SerializeField] float speed = 100.0f;
    [SerializeField] float turnSpeed = 150.0f;
    Vector3 direction;
    Vector2 targetDirection;
    Vector3 rightPerpendicular_direction;

    [SerializeField] GameObject body;
    float distanceToWaypoint;

    [SerializeField] float destinationRadius;

    [SerializeField] GameObject blood;
    bool stopMove = false;

    CameraShake cameraShake;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hit_audioClip;
    Wave wave;

    [SerializeField] bool canDie;
    private void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        wave = GetComponentInParent<Wave>();

        ChangeDirection(transform.forward);
    }


    public void StartFollowPath(List<Vector3> path) 
    {
        followingPath = true;
        waypoint_index = 0;
        waypoints = path;
    }

    void ChangeDirection(Vector3 p_direction) 
    {
        Vector3 dir = p_direction.normalized;
        direction = dir;
        rightPerpendicular_direction = new Vector3(dir.z, 0, dir.x * -1);
    }

    private void Update()
    {
        if(waypoints == null) 
        {
            return;
        }

        if (followingPath)
        {
            FollowPath();
        }        
    }

    void FollowPath() 
    {
        if (ProcessTurn()) 
        {
            if (!stopMove)
            {
               transform.position += direction * speed * Time.deltaTime;
            }

            body.transform.forward = direction;
        }
    }

    Vector3 RotateVector(Vector3 vector, float angle) 
    {
        Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
        return rotatedVector;
    }

    bool ProcessTurn() 
    {
        Vector2 curPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 waypoint = new Vector2(waypoints[waypoint_index].x, waypoints[waypoint_index].z);
        targetDirection = waypoint - curPos;
        distanceToWaypoint = targetDirection.magnitude;
        targetDirection = targetDirection.normalized;
        Vector2 rightVector = new Vector2(rightPerpendicular_direction.x, rightPerpendicular_direction.z);


        //compare right vector to target vector        
        float angle = Mathf.Acos((Vector2.Dot(targetDirection, rightVector)) / 
                      (targetDirection.magnitude * rightVector.magnitude));

        angle = Mathf.Rad2Deg * angle;

        if((angle < 120) && (angle > 60)) 
        {
            stopMove = false;
        }
        else 
        {
            stopMove = true;
        }

        if (angle < 90)//turn left
        {
            if(90 - angle > 5) 
            {
                ChangeDirection(RotateVector(direction, turnSpeed * Time.deltaTime));
            }
            
        }
        else //turn right
        {
            if (angle - 90 > 5)
            {
                ChangeDirection(RotateVector(direction, -turnSpeed * Time.deltaTime));
            }
            
        }

        if(waypoint_index > waypoints.Count - 2)//coming to last waypoint 
        {
            if(distanceToWaypoint < destinationRadius)
            {
                CompletePath();
                return false;
            }
        }

        if (distanceToWaypoint < 1.5f)//coming to waypoint
        {
            waypoint_index++;
            if (waypoint_index > waypoints.Count - 1)
            {
                CompletePath();
            }
        }

        return true;
    }

    public void CompletePath()
    {
        StopFollowingPath();
    }

    void StopFollowingPath()
    {
        followingPath = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDie) 
        {
            return;
        }

        if (other.CompareTag("AttackBox"))
        {
            print("ouch");
            Transform t = new GameObject().transform;
            t.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(blood, t.position, Quaternion.identity);
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.4f));
            audioSource.PlayOneShot(hit_audioClip, 0.3f);
            if(wave != null) 
            {
                wave.UnitDeath();
            }
            Destroy(gameObject);
        }
    }

}


