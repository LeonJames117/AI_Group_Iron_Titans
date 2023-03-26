using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] AStar_Pathfinding a;
    [SerializeField] GameObject end;
    Nav_Agent nav_agent;

    bool pathed = false;

    float updateTimer = 0.0f;
    float updateFrequency = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        nav_agent = GetComponent<Nav_Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
        if(updateTimer >= updateFrequency) 
        {
            updateTimer = 0.0f;
            UpdateController();
        }

    }

    void UpdateController() 
    {
        pathed = true;

        Vector3 startPos = transform.position;
        startPos.y = 0;

        Vector3 endPos = end.transform.position;
        endPos.y = 0;

        if((startPos - endPos).magnitude > 2.0) 
        {

            nav_agent.StartFollowPath(PathfindRequestManager.instance.RequestPath(startPos, endPos));
        }

    }
}
