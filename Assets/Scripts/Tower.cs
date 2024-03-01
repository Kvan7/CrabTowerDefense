using UnityEngine;

public class Tower : MonoBehaviour
{
	public GameObject projectilePrefab;
	public float fireRate = 1f;
	private float fireCountdown = 0f;
	private Transform target;
	private float rotationSpeed = 1f;
	public bool instantRotation = false;

	void Update()
	{
		if (target == null)
			return;

		if (fireCountdown <= 0f)
		{
			Shoot();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;

		// also look at the target
		// Rotate the tower to face the target
		if (instantRotation)
		{
			Vector3 targetDirection = target.position - transform.position;
			transform.rotation = Quaternion.LookRotation(targetDirection);
		}
		else
		{
			Vector3 targetDirection = target.position - transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
		}


	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && target == null)
		{
			target = other.transform;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.transform == target)
		{
			target = null;
		}
	}

	void Shoot()
	{
		GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectile = projectileGO.GetComponent<Projectile>();

		if (projectile != null)
			projectile.Seek(target);
	}
}
