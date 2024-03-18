using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public SuperWave currentWave;
	public GameObject path;
	public UnityEvent onWaveComplete;

	private Vector3 spawnOrigin;
	private Coroutine waveCoroutine;

	private void Start()
	{
		spawnOrigin = spawnPoint.position;
	}

	public void StartWave(SuperWave wave)
	{
		currentWave = wave;
		if (waveCoroutine == null)
		{
			waveCoroutine = StartCoroutine(SpawnWaveItems(currentWave.waveItems, currentWave.endDelay, () =>
			{
				// Invoke the event when the wave is complete
				onWaveComplete.Invoke();
			}));
		}
	}

	IEnumerator SpawnWaveItems(List<SuperWave.WaveItem> waveItems, float groupEndDelay, UnityAction onComplete = null)
	{
		foreach (var item in waveItems)
		{
			// Ensure repeatCount is at least 1
			int repeatCount = Mathf.Max(1, item.repeatCount);

			for (int i = 0; i < repeatCount; i++)
			{
				if (item.isGroup)
				{
					yield return StartCoroutine(SpawnWaveItems(item.children, item.endDelay));
					// No additional delay here as the last item in children controls the delay
				}
				else
				{
					yield return SpawnEnemy(item.prefab.gameObject, item.endDelay);
				}
			}
		}
		yield return new WaitForSeconds(groupEndDelay);

		onComplete?.Invoke();
	}


	IEnumerator SpawnEnemy(GameObject prefab, float endDelay)
	{
		GameObject enemy = Instantiate(prefab, spawnOrigin, Quaternion.identity);
		enemy.GetComponent<FollowWaypoints>().path = path;
		yield return new WaitForSeconds(endDelay);
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
