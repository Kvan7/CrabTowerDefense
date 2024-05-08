using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class GattlingShoot : updateRotate
{
	public Tower gattlingTower;
	[SerializeField] private float shotsPer360 = 7f; // Angle required for each barrel to reach the top
	private float lastShotAngle = 0f; // The last angle at which we fired

	private void OnDestroy()
	{
		rotateWheel.onValueChange.RemoveAllListeners();
	}
	public override void Rot(float value)
	{
		base.Rot(value);
		// Check if the current rotation value has surpassed the next angle for a barrel
		if (Mathf.Abs(value - lastShotAngle) >= (1 / shotsPer360))
		{
			// Adjust to keep within a cycle of 360 degrees
			lastShotAngle = value;

			// Fire the Gatling tower
			gattlingTower.Shoot();
		}
	}
}
