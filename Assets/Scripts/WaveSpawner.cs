using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public WaveBase currentWave;
	public GameObject path;
	public UnityEvent onWaveComplete;

	private Vector3 spawnOrigin;
	private Coroutine waveCoroutine;

	private void Start()
	{
		spawnOrigin = spawnPoint.position;
	}

	public void StartWave(WaveBase wave)
	{
		currentWave = wave;
		if (waveCoroutine == null)
		{
			waveCoroutine = StartCoroutine(SpawnWave(currentWave, () =>
			{
				// Invoke the event when the wave is complete
				onWaveComplete.Invoke();
			}));
		}
	}

	IEnumerator SpawnWave(WaveBase wave, UnityAction onComplete = null)
	{
		if (wave is WaveLeaf leaf)
		{
			yield return SpawnLeaf(leaf);
		}
		else if (wave is Wave waveGroup)
		{
			foreach (var element in waveGroup.waveElements)
			{
				yield return StartCoroutine(SpawnWave(element));
				yield return new WaitForSeconds(waveGroup.endDelay);
			}
		}
		else if (wave is WaveRepeater repeater)
		{
			for (int i = 0; i < repeater.repeatCount; i++)
			{
				foreach (var element in repeater.waveElements)
				{
					yield return StartCoroutine(SpawnWave(element));
				}
			}
			yield return new WaitForSeconds(repeater.endDelay);
		}

		onComplete?.Invoke();
	}

	IEnumerator SpawnLeaf(WaveLeaf leaf)
	{
		GameObject enemy = Instantiate(leaf.selectedEnemyPrefab, spawnOrigin, Quaternion.identity);
		// Assuming you have a component named FollowWaypoints for path following
		enemy.GetComponent<FollowWaypoints>().path = path;
		yield return new WaitForSeconds(leaf.endDelay);
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
