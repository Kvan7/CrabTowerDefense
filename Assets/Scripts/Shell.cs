using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	Rigidbody rb;
	public float explodeRadius = 5f;
	public float damage = 10f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		// Check if the shell is moving
		if (rb.velocity != Vector3.zero)
		{
			// Calculate the rotation needed for the shell to face its direction of travel
			Quaternion rotation = Quaternion.LookRotation(rb.velocity.normalized);

			Quaternion additionalRotation = Quaternion.Euler(90, 0, 0); // 90 degrees rotation around the x-axis

			// Apply the combined rotation to the shell
			rb.MoveRotation(rotation * additionalRotation);
		}
	}

	public void Explode()
	{
		// Get all colliders within the explodeRadius
		Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

		foreach (Collider collider in colliders)
		{
			if (collider.gameObject.CompareTag("Enemy"))
			{
				// Get the enemy script
				Enemy enemy = collider.gameObject.GetComponent<Enemy>();

				// Deal damage to the enemy
				enemy.TakeDamage((int)damage);
			}
		}

		// Play the explosion effect
		GetComponent<ShellEffectHandler>().PlayExplodeEffect();
		// Destroy the shell
		Destroy(gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explodeRadius);
	}
}
