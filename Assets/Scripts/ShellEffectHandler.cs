using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellEffectHandler : MonoBehaviour
{
	public float explodeRadius = 5f;
	public GameObject explosionEffect;
	public GameObject trailEffect;
	// Start is called before the first frame update
	void Start()
	{
		explodeRadius = GetComponent<Shell>().explodeRadius;
		if (trailEffect != null)
		{
			// trailEffect.Play();
		}
	}
	public void PlayExplodeEffect()
	{
		if (explosionEffect != null)
		{
			GameObject particle = Instantiate(explosionEffect, transform.position, Quaternion.identity);
			particle.transform.localScale = new Vector3(0.05f * explodeRadius, 0.05f * explodeRadius, 0.05f * explodeRadius);
			Destroy(particle, 7f);
		}
	}
}
