using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class FollowWaypoints : NetworkBehaviour
{
	public GameObject path;
	int currentWP = 0;
	bool endOfPath = false;

	public float moveSpeed = 7.0f;
	public float rotSpeed = 10.0f;

	public float waypointCompleteDistance = 0.5f;

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
		if (!endOfPath)
		{
			if (Vector3.Distance(transform.position, waypoints[currentWP].transform.position) < waypointCompleteDistance)
			{
				currentWP++;
			}

			if (currentWP >= waypoints.Count)
			{
				endOfPath = true;
				transform.Translate(0, 0, 0);
				gameObject.GetComponent<Enemy>().CompletedPath();
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
