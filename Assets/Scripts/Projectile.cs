using UnityEngine;
using System.Collections; // Required for coroutines

public class Projectile : MonoBehaviour
{
	private float speed = 10f;
	private float damage = 50f;
	private float lifetime = 5f; // Lifetime of the projectile in seconds
	public GameObject particleEffectPrefab; // Reference to your particle system prefab

	private void Start()
	{
		Destroy(gameObject, lifetime); // Destroy the projectile after a certain time
	}

	public void TowerSettings(float speed, float damage, float lifetime)
	{
		this.speed = speed;
		this.damage = damage;
		this.lifetime = lifetime;
	}

	public void Initialize(Vector3 targetPosition)
	{
		if (TryGetComponent<Rigidbody>(out var rb))
		{
			Vector3 initialDirection = targetPosition - transform.position;
			rb.velocity = initialDirection.normalized * speed;

			rb.useGravity = false;
			rb.drag = 0;

			if (initialDirection != Vector3.zero)
			{
				transform.forward = initialDirection.normalized;
			}
		}
		else
		{
			Debug.LogWarning("Projectile does not have a Rigidbody component.");
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.TryGetComponent<Enemy>(out var enemy))
		{
			enemy.TakeDamage((int)damage);
		}
		else
		{
			Debug.LogWarning("Projectile collided with an object that is not an enemy.");
			Debug.LogWarning($"Object name: {collision.collider.name}");
		}

		// Instantiate and play the particle effect at the point of collision
		if (particleEffectPrefab != null)
		{
			GameObject effect = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
			ParticleSystem particles = effect.GetComponent<ParticleSystem>();
			if (particles != null)
			{
				StartCoroutine(DestroyAfterPlay(particles));
			}
			else
			{
				Destroy(effect); // If no ParticleSystem is found, destroy the effect object immediately
			}
		}

		Destroy(gameObject); // Destroy the projectile upon impact
	}

	IEnumerator DestroyAfterPlay(ParticleSystem ps)
	{
		yield return new WaitWhile(() => ps.isPlaying);
		Destroy(ps.gameObject);
	}
}
