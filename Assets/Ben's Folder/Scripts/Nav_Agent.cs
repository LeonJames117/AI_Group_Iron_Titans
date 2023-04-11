using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_Agent : MonoBehaviour
{
    List<Vector3> waypoints;
    int waypoint_index = 0;
    bool followingPath = false;
    float tolerance = 0.1f;
    [SerializeField] float speed = 100.0f;
    Vector3 direction;
    Vector2 targetDirection;
    Vector3 rightPerpendicular_direction;

    [SerializeField] GameObject body;
    [SerializeField] float pathSmoothingBoundary;
    float distanceToWaypoint;
    Vector2 startSmoothDir;
    Vector2 endSmoothDir;
    bool smoothing;
    float smoothDelta = 0.0f;

    [SerializeField] Nav_Grid ng;

    [SerializeField] float destinationRadius;

    [SerializeField] GameObject blood;
    float turnSpeed = 1.0f;
    bool stopMove = false;

    [SerializeField] CameraShake cameraShake;

    //LineA
    //Line lineA;
    //Line perp_lineA;
    //Vector2 lineA_BoundaryIntercept;

    ////LineB
    //Line lineB;
    //Line perp_lineB;
    //Vector2 lineB_BoundaryIntercept;

    //float startSmoothingAngle;
    //float endSmoothingAngle;

    [SerializeField] float smoothTurnSpeed = 2.0f;

    private void Start()
    {
        ChangeDirection(transform.forward);
        //lineA = new Line(waypoints[waypoint_index], waypoints[waypoint_index + 1]);
    }

    void CalculateWaypointLines() 
    {
        //Vector3 startA = waypoints[waypoint_index - 1];
        //Vector3 endA = waypoints[waypoint_index];

        //Vector3 startB = waypoints[waypoint_index];
        //Vector3 endB = waypoints[waypoint_index + 1];

        //Vector2 startPointA = new Vector2(startA.x, startA.z);
        //Vector2 endPointA = new Vector2(endA.x, endA.z);
        //lineA = new Line(startPointA, endPointA);

        //Vector2 startPointB = new Vector2(startB.x, startB.z);
        //Vector2 endPointB = new Vector2(endB.x, endB.z);
        //lineB = new Line(startPointB, endPointB);
    }

    void CalculatePerpendicularBoundaries() 
    {
        ////Calculate perpendicular line to line A
        //float lineA_displaceX = lineA.gradient * pathSmoothingBoundary;
        //float lineA_displaceY = (1 / lineA.gradient) * pathSmoothingBoundary;

        //lineA_BoundaryIntercept = new Vector2(lineA.endPoint.x - lineA_displaceX, lineA.endPoint.y - lineA_displaceY);
        //perp_lineA = lineA.CalculatePerpendicularLine(lineA_BoundaryIntercept);
        
        ////Calculate perpendicular line to line B
        //float lineB_displaceX = lineB.gradient * pathSmoothingBoundary;
        //float lineB_displaceY = (1 / lineB.gradient) * pathSmoothingBoundary;

        //lineB_BoundaryIntercept = new Vector2(lineB.endPoint.x - lineB_displaceX, lineB.endPoint.y - lineB_displaceY);
        //perp_lineB = lineB.CalculatePerpendicularLine(lineB_BoundaryIntercept);
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
            //debug
            List<Node> n = new List<Node>();
            n.Add(ng.GetNodeFromPosition(new Vector3(waypoints[waypoint_index].x, 0, waypoints[waypoint_index].z)));
            ng.path = n;
            //debug

            FollowPath();
        }        
    }

    void FollowPath() 
    {
        //ProcessArrivedAtWaypoint();

        if (ProcessTurn()) 
        {
            //Vector3 td = new Vector3(targetDirection.x, 0, targetDirection.y);

            //float angle = Mathf.Acos((Vector2.Dot(direction, td)) /
            //              (direction.magnitude * td.magnitude));

            //angle = Mathf.Rad2Deg * angle;
            //print("angle: " + angle);

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

        //print("dist: " + distanceToWaypoint);

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
            ChangeDirection(RotateVector(direction, 200 * Time.deltaTime));
            //body.transform.forward = direction;
        }
        else //turn right
        {
            ChangeDirection(RotateVector(direction, -200 * Time.deltaTime));
            //body.transform.forward = direction;
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

    void CompletePath()
    {
        StopFollowingPath();
    }

    void StopFollowingPath()
    {
        followingPath = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttackBox") 
        {
            print("ouch");
            Transform t = new GameObject().transform;
            t.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(blood, t.position, Quaternion.identity);
            cameraShake.StartCoroutine(cameraShake.Shake(0.1f, 0.3f));
            Destroy(gameObject);
        }
    }

}


