using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllableTower : Tower
{
	public XRGrabInteractable turretHead;
	public XRGrabInteractable turretParent;
	public bool playerControlled = false;
	// Start is called before the first frame update
	void Start()
	{
		isMoveable = false;
	}

	void Update()
	{
		if (playerControlled)
		{
			// Rotate the turret head
			if (turretHead.isSelected)
			{
			}
		}
	}

	public void GrabControl(SelectEnterEventArgs args)
	{
		Debug.Log("Grab Control");
		playerControlled = true;
		StopCoroutine(shootCoroutine);
		shootCoroutine = null;
		// lookAtObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}
	public void UnGrabControl(SelectExitEventArgs args)
	{
		Debug.Log("UnGrab Control");
		playerControlled = false;
		shootCoroutine = StartCoroutine(CheckForEnemies());
		// lookAtObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	public void ActivateControl(ActivateEventArgs args)
	{
		Debug.Log("Activate Control");
		shootCoroutine = StartCoroutine(PlayerShoot());
	}

	public void DeactivateControl(DeactivateEventArgs args)
	{
		Debug.Log("Deactivate Control");
		StopCoroutine(shootCoroutine);
		shootCoroutine = null;
	}

	IEnumerator PlayerShoot()
	{
		while (true)
		{
			// BUG: can spam click to shoot quickly
			Shoot(2 * projectileSpeed, 2 * attackDamage, projectileLifetime);
			yield return new WaitForSeconds(1 / (3 * fireRate));
		}
	}
}
