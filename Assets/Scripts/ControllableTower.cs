using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllableTower : Tower
{
	public XRGrabInteractable turretHead;
	public XRGrabInteractable turretParent;
	// Start is called before the first frame update
	void Start()
	{

	}

	public void GrabControl(SelectEnterEventArgs args)
	{
		// lookAtObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}
	public void UnGrabControl(SelectExitEventArgs args)
	{
		// lookAtObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
}
