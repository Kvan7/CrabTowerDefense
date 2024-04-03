using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : NetworkBehaviour
{
	public Transform spawnPoint;
	public GameObject path;
	public UnityEvent onWaveComplete;

	private Vector3 spawnOrigin;
	private Coroutine waveCoroutine;

	private int currentWaveIndex = 0; // Start with the first wave
	private string wavesFolder = "Waves"; // Folder within Resources where waves are stored


	private void Start()
	{
		spawnOrigin = spawnPoint.position;
	}

	[Command(requiresAuthority = false)]
	public void StartNextWave()
	{
		// Check if there are still enemies left before starting the next wave
		if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
		{
			Debug.LogWarning("Cannot start next wave, enemies still present.");
			return;
		}
		if (waveCoroutine != null)
		{
			Debug.LogWarning("Cannot start next wave, wave already in progress.");
			return;
		}

		LoadAndStartWave(++currentWaveIndex);
	}

	private void LoadAndStartWave(int waveIndex)
	{
		Wave wave = Resources.Load<Wave>($"{wavesFolder}/Wave {waveIndex}");
		if (wave == null)
		{
			Debug.LogError($"Wave {waveIndex} not found. Check if all waves are properly configured.");
			return;
		}

		if (waveCoroutine != null)
		{
			StopCoroutine(waveCoroutine); // Ensure any existing wave coroutines are stopped before starting a new one
		}

		waveCoroutine = StartCoroutine(SpawnWaveItems(wave.waveItems, wave.endDelay, () =>
		{
			StartCoroutine(CheckForRemainingEnemies());
		}));
	}

	IEnumerator SpawnWaveItems(List<Wave.WaveItem> waveItems, float groupEndDelay, UnityAction onComplete = null)
	{
		foreach (var item in waveItems)
		{
			int repeatCount = Mathf.Max(1, item.repeatCount);

			for (int i = 0; i < repeatCount; i++)
			{
				if (item.isGroup)
				{
					// For group items, do not pass the onComplete action to avoid premature checks
					yield return StartCoroutine(SpawnWaveItems(item.children, item.endDelay));
				}
				else
				{
					yield return SpawnEnemy(item.enemyData.prefab.gameObject, item.endDelay);
				}
			}
		}

		yield return new WaitForSeconds(groupEndDelay);

		// Invoke the onComplete action if provided, which includes the logic to check for remaining enemies
		onComplete?.Invoke();
	}

	IEnumerator SpawnEnemy(GameObject prefab, float endDelay)
	{
		GameObject enemy = Instantiate(prefab, spawnOrigin, Quaternion.identity);
		enemy.GetComponent<FollowWaypoints>().path = path;
		NetworkServer.Spawn(enemy);
		Debug.Log(enemy.GetComponent<Enemy>().health);
		yield return new WaitForSeconds(endDelay);
	}

	private IEnumerator CheckForRemainingEnemies()
	{
		Debug.Log("Checking for remaining enemies...");
		yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
		Debug.Log("No remaining enemies found, invoking onWaveComplete.");
		onWaveComplete.Invoke();
		waveCoroutine = null;
	}

	public void StopWave()
	{
		if (waveCoroutine != null)
		{
			StopCoroutine(waveCoroutine);
			waveCoroutine = null;
		}
	}
}
