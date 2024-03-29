using System.Collections;
using Mirror;
using UnityEngine;

public class Tower : MonoBehaviour
{
	public GameObject projectilePrefab;
	#region Tower Props
	// Move to scriptable object later
	public float fireRate = 1f;
	public float attackDamage = 50f;
	public float projectileSpeed = 10f;
	public float projectileLifetime = 5f;
	[SerializeField] private float attackRange = 10f; // The range within which to look for enemies
	#endregion
	private Transform target;
	private float rotationSpeed = 1f;
	public bool instantRotation = false;
	public GameObject rangeIndicator; // Assign this in the editor
	public GameObject lookAtObject; // Assign this in the editor

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
			yield return new WaitForSeconds(1 / fireRate);
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


		target = closestEnemy;

		if (target != null && !isMoveable)
		{
			Shoot();
		}
	}


	void Shoot()
	{
		if (isMoveable)
		{
			return;
		}

		if (target == null)
		{
			Debug.LogWarning("Attempted to shoot but no target was set.");
			return;
		}

		GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		if (projectileObject == null)
		{
			Debug.LogError("Failed to instantiate projectile. Check the projectile prefab.");
			return;
		}

		Projectile projectile = projectileObject.GetComponent<Projectile>();
		if (projectile == null)
		{
			Debug.LogError("Instantiated object does not have a Projectile component.");
			Destroy(projectileObject);
			return;
		}

		projectile.TowerSettings(projectileSpeed, attackDamage, projectileLifetime);
		projectile.Initialize(target.position);
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
