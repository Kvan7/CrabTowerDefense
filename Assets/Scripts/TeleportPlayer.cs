using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
	private Transform player;
	public Transform teleportLocation;
	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("XR Origin").transform;
	}

	public void TeleportPlayerToTower()
	{
		player.position = teleportLocation.position;
	}
}
