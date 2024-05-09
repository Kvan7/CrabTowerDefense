using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class MortarTower : AbstractTower
{
	[SerializeField] private Transform targetZone;
	[SerializeField] private Transform rotatingObjectParent;
	[SerializeField] private TowerInfo towerInfo;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform tube;
	[SerializeField] private XRKnob rotateWheel;
	[SerializeField] private XRKnob rangeWheel;
	[SerializeField] private float range = 4.0f;
	[SerializeField] private float fireRate = 0.333f;
	[SerializeField] private float damage = 10;

	private Coroutine shootCoroutine;	
	private bool m_automaticFire = false;
	public bool automaticFire
	{
		get => m_automaticFire;
		set
		{
			m_automaticFire = value;
			if (shootCoroutine != null)
			{
				StopCoroutine(shootCoroutine);
			}
			if (value)
			{
				shootCoroutine = StartCoroutine(ShootCoroutine());
			}
		}

	}
	protected IEnumerator ShootCoroutine()
	{
		while (true)
		{
			Shoot();
			yield return new WaitForSeconds(1 / fireRate);
		}
	}

	public float launchForce = 20.0f;
	// Start is called before the first frame update
	void Start()
	{
		if (towerInfo)
		{
			fireRate = towerInfo.fireRate;
			damage = towerInfo.attackDamage;
			range = towerInfo.attackRange;
		}
		// Set the tower's range indicator to the attack range
		rangeWheel.onValueChange.AddListener((value) =>
		{
			OnRangeChange(value);
		});

		// Set the tower's rotation speed to the rotation speed
		rotateWheel.onValueChange.AddListener((value) =>
		{
			OnRotateChange(value);
		});
		OnRangeChange(rangeWheel.value);
		OnRotateChange(rotateWheel.value);

		// Set the tower's target area radius to the attack range
		targetZone.localScale = new Vector3(towerInfo.attackRange * 2, targetZone.localScale.y * towerInfo.attackRange, towerInfo.attackRange * 2);
	}

	private void OnRangeChange(float value)
	{
		float angleDegrees = value * 30 + 3.0f;
		tube.localRotation = Quaternion.Euler(angleDegrees, 0, 0);

		// Convert angle to radians for calculations
		float launchAngleRad = angleDegrees * Mathf.Deg2Rad;
		float g = Mathf.Abs(Physics.gravity.y);

		// Vertical component of the launch velocity
		float Vv = launchForce * Mathf.Sin(launchAngleRad);

		// Time of flight (up and down)
		float timeOfFlight = 2 * Vv / g;

		// Horizontal component of the launch velocity
		float Vh = launchForce * Mathf.Cos(launchAngleRad);

		// Horizontal distance
		float horizontalDistance = Vh * timeOfFlight;

		// Set the targetZone position based on the calculated horizontal distance
		targetZone.localPosition = new Vector3(0, 0, horizontalDistance);
	}

	private void OnRotateChange(float value)
	{
		rotatingObjectParent.localRotation = Quaternion.Euler(0, value * 36, 0);
	}

	private void OnDestroy()
	{
		rangeWheel.onValueChange.RemoveAllListeners();
		rotateWheel.onValueChange.RemoveAllListeners();
	}
	public void Shoot()
	{
		// // Instantiate the projectile at the tube's position and orientation
		// GameObject projectile = Instantiate(projectilePrefab, tube.position, Quaternion.identity);
		// Rigidbody rb = projectile.GetComponent<Rigidbody>();
		// Shell shell = projectile.GetComponent<Shell>();
		// shell.explodeRadius = range;
		// shell.damage = damage;

		// // Apply the force in the tube's upward direction
		// Vector3 force = tube.up * launchForce;

		// // Apply the force to the projectile
		// rb.AddForce(force, ForceMode.Impulse);
		// Instantiate the projectile at the tube's position and orientation
		GameObject projectile = Instantiate(projectilePrefab, tube.position, Quaternion.identity);
		projectile.GetComponent<Shell>().explodeRadius = range;
		projectile.GetComponent<Shell>().damage = damage;

		// Apply the force in the tube's upward direction
		Vector3 force = tube.up * launchForce;

		// Apply the force to the projectile
		projectile.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		NetworkServer.Spawn(projectile);
	}
}
