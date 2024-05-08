using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{
	public GameObject objectToCreate;
	public Transform spawnLocation;
	private GameObject createdObject;

	public void Create()
	{
		if (createdObject != null)
		{
			Destroy(createdObject);
		}
		createdObject = Instantiate(objectToCreate, spawnLocation.position, spawnLocation.rotation);
		createdObject.SetActive(true);
	}
	public void Destroy()
	{
		Destroy(createdObject);
	}
}
