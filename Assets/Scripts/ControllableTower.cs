using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllableTower : Tower
{
	public XRGrabInteractable turretHead;
	public Transform yawPivot;
	public Transform pitchPivot;
	public Vector3 pitchAxis = Vector3.right;
	public XRGrabInteractable turretParent;
	public Transform handle;
	public bool playerControlled = false;
	private float initialYawOffset;
	private float initialPitchOffset;

	public bool useLimits = false;
	public float minYaw = -90f;
	public float maxYaw = 90f;
	public float minPitch = -45f;
	public float maxPitch = 45f;

	private Coroutine playerShoot;

	protected override void Start()
	{
		CalculateInitialOffsets();
		base.Start();
	}

	protected override void Update()
	{
		if (playerControlled && turretHead.isSelected)
		{
			Transform hand = turretHead.interactorsSelecting.First().transform;
			UpdateTurretRotation(hand);
		}

		base.Update();
	}

	void CalculateInitialOffsets()
	{
		// Calculate the initial yaw offset
		Vector3 directionToHandleFlat = handle.position - turretHead.transform.position;
		directionToHandleFlat.y = 0;
		initialYawOffset = -Vector3.SignedAngle(turretHead.transform.forward, directionToHandleFlat, Vector3.up);

		// Calculate the initial pitch offset
		Vector3 directionToHandle = handle.position - turretHead.transform.position;
		initialPitchOffset = Vector3.SignedAngle(directionToHandleFlat.normalized, directionToHandle.normalized, turretHead.transform.right);
	}

	void UpdateTurretRotation(Transform hand)
	{
		Transform turretBase = turretHead.transform;

		// Yaw control
		Vector3 directionToHandFlat = hand.position - turretBase.position;
		directionToHandFlat.y = 0;
		Quaternion yawRotation = Quaternion.Euler(0, initialYawOffset, 0) * Quaternion.LookRotation(directionToHandFlat.normalized, Vector3.up);

		// Extract the yaw angle after rotation is applied to check against limits
		float currentYawAngle = yawRotation.eulerAngles.y;
		if (useLimits)
		{
			currentYawAngle = Mathf.Clamp(currentYawAngle - 360 * (int)(currentYawAngle / 360), minYaw, maxYaw);  // Clamp and handle full rotations
			yawRotation = Quaternion.Euler(0, currentYawAngle, 0);
		}
		yawPivot.rotation = yawRotation;

		// Pitch control
		Vector3 directionToHand = hand.position - turretBase.position;
		float currentPitchAngle = Vector3.SignedAngle(directionToHandFlat.normalized, directionToHand.normalized, turretBase.right);
		float pitchAngleWithOffset = currentPitchAngle - initialPitchOffset;

		if (useLimits)
		{
			pitchAngleWithOffset = Mathf.Clamp(pitchAngleWithOffset, minPitch, maxPitch);
		}

		Quaternion pitchRotation = Quaternion.Euler(pitchAngleWithOffset * pitchAxis.x, pitchAngleWithOffset * pitchAxis.y, pitchAngleWithOffset * pitchAxis.z);

		// Apply yaw and pitch rotations to the turret base
		pitchPivot.localRotation = pitchRotation;
		if (pitchPivot.gameObject.GetInstanceID() == yawPivot.gameObject.GetInstanceID())
		{
			yawPivot.rotation = yawRotation * pitchRotation;
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
		if (shootCoroutine != null)
			StopCoroutine(shootCoroutine);
		if (playerShoot != null)
		{
			StopCoroutine(playerShoot);
			playerShoot = null;
		}
		shootCoroutine = StartCoroutine(CheckForEnemies());
	}
	public void ActivateControl(ActivateEventArgs args)
	{
		Debug.Log("Activate control");
		if (playerShoot != null)
			StopCoroutine(playerShoot);
		playerShoot = StartCoroutine(PlayerShoot());
	}

	public void DeactivateControl(DeactivateEventArgs args)
	{
		if (playerShoot != null)
			StopCoroutine(playerShoot);
		playerShoot = null;
	}

	IEnumerator PlayerShoot()
	{
		while (true)
		{
			// BUG: can spam click to shoot quickly
			Shoot(projectileSpeed, attackDamage, projectileLifetime);
			yield return new WaitForSeconds(1 / (3 * fireRate));
		}
	}
}
