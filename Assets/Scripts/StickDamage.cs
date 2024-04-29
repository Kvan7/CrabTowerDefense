using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickDamage : MonoBehaviour
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.TryGetComponent<Enemy>(out var enemy))
		{
			enemy.TakeDamage(1);
		}
	}
}
