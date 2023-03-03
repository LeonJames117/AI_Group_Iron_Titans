using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] GameObject end;
    Nav_Agent nav_agent;

    // Start is called before the first frame update
    void Start()
    {
        nav_agent = GetComponent<Nav_Agent>();

        Vector3 startPos = transform.position;
        startPos.y = 0;

        Vector3 endPos = end.transform.position;
        endPos.y = 0;

        nav_agent.StartFollowPath(PathfindRequestManager.instance.RequestPath(startPos, endPos));
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}
