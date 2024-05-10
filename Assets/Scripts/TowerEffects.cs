using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TowerEffects : NetworkBehaviour
{
	private AbstractTower towerComponent;

	[SerializeField] private GameObject shootEffectPrefab;
	[SerializeField] private Transform shootEffectSpawnPoint;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip shootSoundClip;
	[SerializeField] private float effectDuration = 2.0f; // Duration after which the effect is destroyed

	void Start()
	{
		towerComponent = GetComponent<AbstractTower>();
		if (towerComponent == null)
		{
			Debug.LogError("TowerEffects requires a component that extends AbstractTower!");
			this.enabled = false;  // Disable this component if the requirement is not met
			return;
		}
		towerComponent.OnShot += OnShoot;
	}
	private void OnDestroy()
	{
		if (towerComponent == null)
		{
			return;
		}
		towerComponent.OnShot -= OnShoot;
	}

	private void OnShoot()
	{
		CreateShootEffect();
		PlayShootSound();
	}

	private void CreateShootEffect()
	{
		if (shootEffectPrefab != null && shootEffectSpawnPoint != null)
		{
			GameObject shootEffect = Instantiate(shootEffectPrefab, shootEffectSpawnPoint.position, shootEffectSpawnPoint.rotation);
			Destroy(shootEffect, effectDuration);
		}
		else
		{
			Debug.LogWarning("Shoot effect prefab or spawn point is not set.");
		}
	}

	private void PlayShootSound()
	{
		if (audioSource != null && shootSoundClip != null)
		{
			audioSource.PlayOneShot(shootSoundClip);
		}
		else
		{
			Debug.LogWarning("Audio source or shoot sound clip is missing.");
		}
	}

}
