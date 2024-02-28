using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    public GameObject path;
    int currentWP = 0;
    bool endOfPath = false;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 5.0f;
    
    public float waypointCompleteDistance = 1.0f;

    private List<GameObject> waypoints = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in path.transform)
        {
            waypoints.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if(!endOfPath)
        {
            if(Vector3.Distance(transform.position, waypoints[currentWP].transform.position) < waypointCompleteDistance)
            {
                currentWP++;
            }

            if(currentWP >= waypoints.Count)
            {
                endOfPath = true;
                transform.Translate(0, 0, 0);
            }
            else
            {
                Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWP].transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

                transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            }
        }
        
    }
}
