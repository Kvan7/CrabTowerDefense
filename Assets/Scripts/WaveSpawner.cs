using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	public List<GameObject> enemiesList;

	public GameObject path;

	private Vector3 spawnOrigin;
	public Transform spawnPoint;

	private Dictionary<string, GameObject> enemies;
	private Coroutine waveCoroutine;

	public void StartWave()
	{
		if (waveCoroutine == null)
		{
			spawnOrigin = spawnPoint.position;
			enemies = enemiesList.ToDictionary(x => x.name, x => x);
			waveCoroutine = StartCoroutine(SpawnEnemy());
		}
		// spawnOrigin = spawnPoint.position;
		// enemies = enemiesList.ToDictionary(x => x.name, x => x);
		// waveCoroutine = StartCoroutine(SpawnEnemy());
	}

	IEnumerator SpawnEnemy()
	{
		for (int i = 0; i < 10; i++)
		{
			GameObject enemy = Instantiate(enemies["Enemy"], spawnOrigin, Quaternion.identity);
			enemy.GetComponent<FollowWaypoints>().path = path;
			yield return new WaitForSeconds(2);
		}
	}
}
