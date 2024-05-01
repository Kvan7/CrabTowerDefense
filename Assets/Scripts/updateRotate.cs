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
	public void Rot(float value)
	{
		toRotate.rotation = Quaternion.Euler(0, 0, 180 * value);
	}
}
