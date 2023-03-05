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

    public void StartFollowPath(List<Vector3> path) 
    {
        followingPath = true;
        waypoint_index = 0;
        waypoints = path;

        int i = 0;

        foreach (Vector3 item in waypoints)
        {
            print("waypoint " + i + ": " + item);
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


    }

    void ProcessArrivedAtWaypoint() 
    {
        Vector3 dir = waypoints[waypoint_index] - transform.position;
        dir.y = 0;
        float distance = dir.magnitude;
        dir = Vector3.Normalize(dir);
        direction = dir;

        if (distance < 0.01f) 
        {
            waypoint_index++;
            if (waypoint_index > waypoints.Count - 1) 
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
