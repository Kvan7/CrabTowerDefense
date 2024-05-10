using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public GameObject prefab; // The prefab to pool
	public int poolSize = 10; // Number of objects in the pool
	private Queue<GameObject> poolQueue = new Queue<GameObject>();

	void Start()
	{
		// Initialize the pool
		for (int i = 0; i < poolSize; i++)
		{
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false); // Start with the object disabled
			poolQueue.Enqueue(obj);
		}
	}

	public GameObject GetPooledObject()
	{
		if (poolQueue.Count > 0)
		{
			GameObject obj = poolQueue.Dequeue();
			obj.SetActive(true);
			return obj;
		}

		// Optionally expand the pool if empty
		GameObject newObj = Instantiate(prefab);
		newObj.SetActive(true);
		return newObj;
	}

	public void ReturnObjectToPool(GameObject obj)
	{
		obj.SetActive(false);
		poolQueue.Enqueue(obj);
	}
}
