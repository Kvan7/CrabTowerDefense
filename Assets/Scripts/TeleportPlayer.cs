using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class TeleportPlayer : MonoBehaviour
{
	private Transform player;
	public Transform teleportLocation;
	[SerializeField] private XRPushButton pushButton;
	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("XR Origin").transform;
	}

	public void TeleportPlayerToTower()
	{
		player.position = teleportLocation.position;
		Debug.Log(pushButton.value);
		Debug.Log(pushButton.interactorsHovering);
		Debug.Log(pushButton.interactorsSelecting);
	}
}
