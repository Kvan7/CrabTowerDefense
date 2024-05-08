using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnDrop : MonoBehaviour
{
	public Transform[] toReset;

	private bool m_lockToValue = false;

	public bool LockToValue
	{
		get => m_lockToValue;
		set => m_lockToValue = value;
	}

	private void Update()
	{
		if (LockToValue)
		{
			Reset();
		}
	}

	public void Reset()
	{
		Debug.Log("Resetting");
		foreach (Transform t in toReset)
		{
			t.rotation = Quaternion.identity;
			Debug.Log("Resetting: " + t.name + " X: " + t.localRotation.x + " Y: " + t.localRotation.y + " Z: " + t.localRotation.z);
		}
	}
}
