using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public GameObject projectilePrefab;
	public float fireRate = 1f;
	// private float fireCountdown = 0f;
	private Transform target;
	private float rotationSpeed = 1f;
	public bool instantRotation = false;
	public GameObject rangeIndicator; // Assign this in the editor
	public GameObject lookAtObject; // Assign this in the editor

	// public float checkInterval = 0.5f; // How often to check for enemies in seconds
	[SerializeField] private float attackRange = 10f; // The range within which to look for enemies
	private bool _isMoveable = true; // Whether the tower can be moved

	public bool isMoveable
	{
		get { return _isMoveable; }
		set
		{
			if (value)
			{
				// If the tower is now moveable, stop shooting
				target = null;
				_isMoveable = value;
				gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			}
			else
			{
				// If the tower is now not moveable, start shooting
				_isMoveable = value;
				gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			}
		}
	}

	void Start()
	{
		StartCoroutine(CheckForEnemies());
	}


	void Update()
	{
		if (target == null)
			return;

		if (isMoveable)
			return;

		// if (fireCountdown <= 0f)
		// {
		// 	Shoot();
		// 	fireCountdown = 1f / fireRate;
		// }

		// fireCountdown -= Time.deltaTime;

		// also look at the target
		// Rotate the tower to face the target
		Vector3 targetDirection = target.position - transform.position;
		if (!instantRotation)
		{
			Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
			lookAtObject.transform.rotation = Quaternion.Slerp(lookAtObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
		}
		else
		{
			lookAtObject.transform.rotation = Quaternion.LookRotation(targetDirection);
		}
	}
	IEnumerator CheckForEnemies()
	{
		while (true)
		{
			UpdateTarget();
			yield return new WaitForSeconds(fireRate);
		}
	}
	void OnDrawGizmos()
	{
		// Set the color of the Gizmo
		Gizmos.color = Color.red; // You can change the color to whatever you prefer

		// Draw a wireframe sphere with the same center and radius as your Physics.OverlapSphere call
		Gizmos.DrawWireSphere(transform.position, attackRange * gameObject.transform.localScale.x);
	}

	void UpdateTarget()
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, attackRange * gameObject.transform.localScale.x);
		float closestDistance = float.MaxValue;
		Transform closestEnemy = null;
		int enemyCount = 0;

		foreach (Collider hit in hits)
		{
			if (hit.gameObject.CompareTag("Enemy"))
			{
				float distance = Vector3.Distance(transform.position, hit.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestEnemy = hit.transform;
				}
				enemyCount++;
			}
		}

		Debug.Log("Closest enemy: " + closestEnemy + " at distance: " + closestDistance + " total hits: " + enemyCount);

		target = closestEnemy;
		Debug.Log("Target: " + target);

		if (target != null && !isMoveable)
		{
			Shoot();
		}
	}


	void Shoot()
	{
		if (isMoveable)
		{
			Debug.LogWarning("Tower is moveable, cannot shoot");
			return;
		}
		GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectile = projectileGO.GetComponent<Projectile>();

		Debug.Log("Shooting at: " + target);
		if (projectile != null)
			projectile.Seek(target);
	}


	void UpdateRangeIndicator()
	{
		if (rangeIndicator != null)
		{
			// Assuming the sphere's original scale is 1 unit in diameter
			// and that the attackRange represents the radius of the desired sphere,
			// the scale factor should be attackRange * 2 (to get diameter),
			// and since the default Unity sphere has a diameter of 1, we don't need to adjust further
			float scaleFactor = attackRange * 2;
			rangeIndicator.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
		}
	}

	void OnValidate()
	{
		UpdateRangeIndicator();
	}
}
