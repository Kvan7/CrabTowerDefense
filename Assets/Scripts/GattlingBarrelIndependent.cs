using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GattlingBarrelIndependent : MonoBehaviour
{
	private ControllableTower gattlingTower;
	[SerializeField] private Transform barrel;
	private float fireRate;
	private float rotationSpeed; // Degrees per second
	private readonly int barrelCount = 7; // Number of barrels on the gattling gun

	void Start()
	{
		gattlingTower = GetComponent<ControllableTower>();
		if (gattlingTower != null && gattlingTower.towerInfo != null)
		{
			fireRate = 1 / gattlingTower.towerInfo.fireRate;
			rotationSpeed = 360.0f / (fireRate * barrelCount); // Total rotation speed to complete one full rotation per 7 shots
		}
		else
		{
			Debug.LogError("ControllableTower component or TowerInfo not found!");
		}
	}

	void Update()
	{
		if (gattlingTower != null && gattlingTower.EnemiesInRange && !gattlingTower.playerControlled)
		{
			// Smooth continuous rotation using Time.deltaTime
			barrel.Rotate(0, 0, rotationSpeed * Time.deltaTime);
		}
	}
}
