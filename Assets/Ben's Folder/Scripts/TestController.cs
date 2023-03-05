using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] AStar_Pathfinding a;
    [SerializeField] GameObject end;
    Nav_Agent nav_agent;

    bool pathed = false;

    // Start is called before the first frame update
    void Start()
    {
        nav_agent = GetComponent<Nav_Agent>();




        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > 2 && pathed == false) 
        {
            pathed = true;

            Vector3 startPos = transform.position;
            startPos.y = 0;

            Vector3 endPos = end.transform.position;
            endPos.y = 0;

            nav_agent.StartFollowPath(PathfindRequestManager.instance.RequestPath(startPos, endPos));
        }

        //Vector3 startPos = transform.position;
        //startPos.y = 0;

        //Vector3 endPos = end.transform.position;
        //endPos.y = 0;

        //a.Pathfind(startPos, endPos);
    }
}
