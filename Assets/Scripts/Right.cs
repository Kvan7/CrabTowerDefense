using UnityEngine;
using UnityEngine.InputSystem;

public class Right : MonoBehaviour
{
	public InputActionReference spawnActionReference; // Reference to your Input Action
	public GameObject prefabToSpawn; // The prefab you want to spawn
	public LayerMask groundLayer; // Layer mask to identify the ground
	public Vector3 spawnOffset; // Offset from the hit point to spawn the prefab

	private void OnEnable()
	{
		spawnActionReference.action.performed += SpawnPrefab;
		spawnActionReference.action.Enable();
	}

	private void OnDisable()
	{
		spawnActionReference.action.Disable();
	}

	private void SpawnPrefab(InputAction.CallbackContext context)
	{
		// RaycastHit hit;
		// if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, groundLayer))
		// {
		Instantiate(prefabToSpawn, spawnOffset, Quaternion.identity);
		// }
	}
}
