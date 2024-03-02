using UnityEngine;
using UnityEngine.InputSystem;

public class Right : MonoBehaviour
{
	public GameObject prefabToSpawn; // The prefab you want to spawn
	public Transform spawnLocation; // Offset from the hit point to spawn the prefab

	public void SpawnPrefab()
	{
		Instantiate(prefabToSpawn, spawnLocation.position, Quaternion.identity);
	}
}
