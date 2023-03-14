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

    [SerializeField] float smoothTurnBoundary = 1.0f;
    float turnedDistance = 0.0f;
    bool beyondTurnBoundary = false;

    [SerializeField] GameObject body;

    public void StartFollowPath(List<Vector3> path) 
    {
        followingPath = true;
        waypoint_index = 0;
        waypoints = path;

        int i = 0;

        foreach (Vector3 item in waypoints)
        {
            i++;
        }
    }

    private void Update()
    {
        if (followingPath) 
        {
            FollowPath();
        }        
    }

    void FollowPath() 
    {
        ProcessArrivedAtWaypoint();
        transform.position += direction * speed * Time.deltaTime;

        if (beyondTurnBoundary) 
        {
            turnedDistance += speed * Time.deltaTime;

        }
        body.transform.forward = direction;
    }

    void ProcessArrivedAtWaypoint() 
    {
        Vector3 dir = waypoints[waypoint_index] - transform.position;
        dir.y = 0;
        float distance = dir.magnitude;
        dir = Vector3.Normalize(dir);
        direction = dir;

        if (((distance < smoothTurnBoundary) || beyondTurnBoundary) &&
           (waypoint_index + 1 < waypoints.Count))
        {
            beyondTurnBoundary = true;

            float delta = (turnedDistance) / (smoothTurnBoundary);

            Vector3 nextWaypointDir = waypoints[waypoint_index + 1] - transform.position;
            nextWaypointDir.y = 0;
            nextWaypointDir = Vector3.Normalize(nextWaypointDir);

            //float delta = 1 - ((smoothTurnBoundary - distance) / smoothTurnBoundary);

            Vector3 newDir = Vector3.Lerp(dir, nextWaypointDir, delta);
            direction = newDir;

            if (delta >= 1)
            {
                turnedDistance = 0;
                waypoint_index++;
                beyondTurnBoundary = false;
            }
        }

        if (waypoint_index == waypoints.Count - 1 && distance < 0.01f) // last waypoint
        {
            //Path Complete
            print("complete");
            CompletePath();
        }


    }

    void CompletePath()
    {
        StopFollowingPath();
    }

    void StopFollowingPath()
    {
        followingPath = false;
    }

}


