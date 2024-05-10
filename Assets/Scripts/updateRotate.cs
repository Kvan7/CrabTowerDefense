using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class updateRotate : MonoBehaviour
{
	public Transform toRotate;
	public XRKnob rotateWheel;

	private void Start()
	{
		rotateWheel.onValueChange.AddListener((value) =>
		{
			Rot(value);
		});
	}
	private void OnDestroy()
	{
		rotateWheel.onValueChange.RemoveAllListeners();
	}
	public virtual void Rot(float value)
	{
		toRotate.localRotation = Quaternion.Euler(toRotate.localRotation.x, toRotate.localRotation.y, 720 * value);
		// Debug.Log("Rotating: " + value + " X: " + toRotate.localRotation.x + " Y: " + toRotate.localRotation.y + " Z: " + toRotate.localRotation.z);
	}
}
