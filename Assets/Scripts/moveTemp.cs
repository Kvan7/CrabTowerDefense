using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTemp : MonoBehaviour
{
	public float speed = 5f;
	// Update is called once per frame
	void Update()
	{
		// move enemeny slowly
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
