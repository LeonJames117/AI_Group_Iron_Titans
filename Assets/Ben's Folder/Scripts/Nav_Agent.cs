using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav_Agent : MonoBehaviour
{
    List<Vector3> waypoints;
    int waypoint_index = 0;
    bool followingPath = false;
    float tolerance = 0.1f;
    float speed = 1.0f;
    Vector3 direction;

    public void StartFollowPath(List<Vector3> path) 
    {
        followingPath = true;
        waypoint_index = 0;
        waypoints = path;
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
    }

    void ProcessArrivedAtWaypoint() 
    {
        Vector3 dir = waypoints[waypoint_index]  - transform.position;
        dir.y = 0;
        float distance = dir.magnitude;
        dir = Vector3.Normalize(dir);

        if (distance < 0.01f) 
        {
            waypoint_index++;
            if (waypoint_index < waypoints.Count) 
            {
                //Path Complete
                CompletePath();
            }
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
