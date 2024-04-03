using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllableTower : Tower
{
	public XRGrabInteractable turretHead;
	public XRGrabInteractable turretParent;
	public Transform handle;
	public bool playerControlled = false;
	private float offsetAngle;
	private float initialYawOffset;
	private float initialPitchOffset;

	void Start()
	{
		// Calculate the initial yaw offset
		Vector3 directionToHandleFlat = handle.position - turretHead.transform.position;
		directionToHandleFlat.y = 0;
		initialYawOffset = -Vector3.SignedAngle(turretHead.transform.forward, directionToHandleFlat, Vector3.up);

		// Calculate the initial pitch offset
		Vector3 directionToHandle = handle.position - turretHead.transform.position;
		initialPitchOffset = Vector3.SignedAngle(directionToHandleFlat.normalized, directionToHandle.normalized, turretHead.transform.right);
		isMoveable = false;
	}

	void Update()
	{
		if (playerControlled && turretHead.isSelected)
		{
			Transform hand = turretHead.interactorsSelecting.First().transform;
			Transform turretBase = turretHead.transform;

			// Yaw control
			Vector3 directionToHandFlat = hand.position - turretBase.position;
			directionToHandFlat.y = 0;
			Quaternion yawRotation = Quaternion.Euler(0, initialYawOffset, 0) * Quaternion.LookRotation(directionToHandFlat.normalized, Vector3.up);

			// Pitch control
			Vector3 directionToHand = hand.position - turretBase.position;
			float currentPitchAngle = Vector3.SignedAngle(directionToHandFlat.normalized, directionToHand.normalized, turretBase.right);
			float pitchAngleWithOffset = currentPitchAngle - initialPitchOffset;

			// Apply yaw and pitch rotations to the turret base
			turretBase.rotation = yawRotation * Quaternion.Euler(pitchAngleWithOffset, 0, 0);
		}
	}


	public void GrabControl(SelectEnterEventArgs args)
	{
		playerControlled = true;
		if (shootCoroutine != null)
			StopCoroutine(shootCoroutine);
		shootCoroutine = null;
	}
	public void UnGrabControl(SelectExitEventArgs args)
	{
		playerControlled = false;
		shootCoroutine = StartCoroutine(CheckForEnemies());
	}
	public void ActivateControl(ActivateEventArgs args)
	{
		shootCoroutine = StartCoroutine(PlayerShoot());
	}

	public void DeactivateControl(DeactivateEventArgs args)
	{
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
