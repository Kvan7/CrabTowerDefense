using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float speed = 70f;
	public float damage = 50f;
	private Transform target;

	void Update()
	{
		if (target == null)
		{
			Destroy(gameObject); // Destroy the projectile if the target is null (e.g., the target was destroyed before the projectile hits it)
			return;
		}

		Vector3 direction = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		// Check if the projectile is close to hitting the target
		if (direction.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		// Move the projectile towards the target
		transform.Translate(direction.normalized * distanceThisFrame, Space.World);
		// Optionally, make the projectile face the target
		transform.LookAt(target);
	}

	void OnCollisionEnter(Collision collision)
	{
		var enemy = collision.collider.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage((int)damage);
			// Apply additional effects like slowing the enemy if needed
		}
		Destroy(gameObject); // Destroy the projectile upon impact
	}

	public void Seek(Transform _target)
	{
		target = _target;
	}

	void HitTarget()
	{
		// Add what happens when the projectile hits the target here
		// For example, apply damage, play an explosion effect, etc.
		var enemy = target.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage((int)damage);
		}
		Destroy(gameObject); // Destroy the projectile upon hitting the target
	}
}
