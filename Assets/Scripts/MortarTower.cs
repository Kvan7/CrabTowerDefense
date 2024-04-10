using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class MortarTower : MonoBehaviour
{
	[SerializeField] private Transform targetZone;
	[SerializeField] private Transform rotatingObjectParent;
	[SerializeField] private TowerInfo towerInfo;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform tube;
	[SerializeField] private XRKnob rotateWheel;
	[SerializeField] private XRKnob rangeWheel;
	[SerializeField] private ParticleSystem explodeParticles;
	[SerializeField] private ParticleSystem muzzleFlash;
	[SerializeField] private float fireRate = 0.333f;
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
				Debug.Log("Starting shoot coroutine");
				shootCoroutine = StartCoroutine(ShootCoroutine());
			}
			else
			{
				Debug.Log("Stopping shoot coroutine");
			}
		}

	}
	protected IEnumerator ShootCoroutine()
	{
		while (true)
		{
			Debug.Log("Attempting to shoot!");
			Shoot();
			yield return new WaitForSeconds(1 / fireRate);
		}
	}

	public float launchForce = 20.0f;
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

		// Set the tower's rotation speed to the rotation speed
		rotateWheel.onValueChange.AddListener((value) =>
		{
			rotatingObjectParent.localRotation = Quaternion.Euler(0, value * 360 / 2, 0);
		});
	}
	private void OnDestroy()
	{
		rangeWheel.onValueChange.RemoveAllListeners();
		rotateWheel.onValueChange.RemoveAllListeners();
	}

	// Update is called once per frame
	void Update()
	{
	}

	void Shoot()
	{
		Debug.Log("Shoot");
		Vector3 toTarget = targetZone.position - tube.position;
		float distance = toTarget.magnitude;
		float g = Mathf.Abs(Physics.gravity.y);

		// Horizontal distance
		float x = distance;
		// Vertical distance
		float y = toTarget.y;

		// Calculate launch angle in radians
		float angle = Mathf.Asin(g * x / (launchForce * launchForce)) / 2.0f;

		// If the angle is a valid number, proceed
		if (!float.IsNaN(angle))
		{
			Debug.Log("Firing solution found! Angle: " + angle * Mathf.Rad2Deg + " degrees.");
			GameObject projectile = Instantiate(projectilePrefab, tube.position, Quaternion.identity);
			Rigidbody rb = projectile.GetComponent<Rigidbody>();

			// Decompose the launch force into horizontal and vertical components
			Vector3 launchDirection = (toTarget.normalized * Mathf.Cos(angle)) + (Vector3.up * Mathf.Sin(angle));
			Vector3 force = launchDirection * launchForce;

			// Apply the force to the projectile
			rb.AddForce(force, ForceMode.Impulse);
		}
		else
		{
			Debug.LogError("No valid firing solution for this target!");
		}
	}
}
