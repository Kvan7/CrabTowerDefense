using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCollider : MonoBehaviour
{
	// Calls parent explode on contact with enemy or terrain
	private bool m_hasExploded = false;
	void OnTriggerEnter(Collider other)
	{
		if (!m_hasExploded && (other.gameObject.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Terrain")))
		{
			m_hasExploded = true;
			Shell shell = GetComponentInParent<Shell>();
			shell.Explode();
		}
	}
}
