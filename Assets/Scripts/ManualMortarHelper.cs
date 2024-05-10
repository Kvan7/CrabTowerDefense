using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class ManualMortarHelper : MonoBehaviour
{
	[SerializeField] private XRLockSocketInteractor manualShellSocket;
	[SerializeField] private Transform bottom;
	[SerializeField] private GameObject fakeShell;
	public void SelectEntered(SelectEnterEventArgs args)
	{
		// Create a fake shell to show the player where the shell will be placed
		GameObject fakeShellInstance = Instantiate(fakeShell, manualShellSocket.gameObject.transform.position, bottom.parent.transform.rotation);
		Destroy(args.interactableObject.transform.gameObject);

		StartCoroutine(MoveShellToTube(fakeShellInstance));
	}

	private IEnumerator MoveShellToTube(GameObject fakeObject)
	{
		// Move shell to bottom of tube over about half a second
		float time = 0.5f;
		float elapsedTime = 0.0f;
		Vector3 startPos = fakeObject.transform.position;
		Vector3 endPos = bottom.position;
		// Debug.Log("start:" + startPos);
		// Debug.Log("end:" + endPos);
		while (elapsedTime < time)
		{
			fakeObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
			elapsedTime += Time.deltaTime;
			// Debug.Log(fakeObject.transform.position);
			yield return null;
		}
		Destroy(fakeObject.transform.gameObject);
		GetComponent<MortarTower>().Shoot();
	}
}
