using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAnimator : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    float jumpSpeed;
    float timer;
    Vector3 orig;
    // Start is called before the first frame update
    void Start()
    {
        orig = transform.position;
        jumpSpeed = Random.Range(1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float y = ((Mathf.Sin(timer * Mathf.PI * 2 * jumpSpeed) + 1) / 2) * jumpHeight;
        transform.position = orig + new Vector3(0, y, 0);
    }
}
