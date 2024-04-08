using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class MortarTower : MonoBehaviour
{
	[SerializeField] private Transform targetZone;
	[SerializeField] private TowerInfo towerInfo;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform tube;
	[SerializeField] private XRKnob rotateWheel;
	[SerializeField] private XRKnob rangeWheel;
	// Start is called before the first frame update
	void Start()
	{
		// Set the tower's range indicator to the attack range
		rangeWheel.onValueChange.AddListener((value) =>
		{
			// Keep the tube rotation as it is
			tube.localRotation = Quaternion.Euler(value * 30, 0, 0);

			// Use the scaledValue for the targetZone.position
			targetZone.localPosition = new Vector3(0, 0, value * 37 + 3);
		});

		// // Set the tower's rotation speed to the rotation speed
		// rotateWheel.onValueChange.AddListener((value) =>
		// {
		// 	gameObject.transform.rotation = Quaternion.Euler(0, value * 360, 0);
		// });
	}
	private void OnDestroy()
	{
		rangeWheel.onValueChange.RemoveAllListeners();
		// rotateWheel.onValueChange.RemoveAllListeners();
	}

	// Update is called once per frame
	void Update()
	{
	}
}
