using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour {

	// Object to look at
	public Transform CenterCube;

	Transform parentTransform;

	void Start()
	{
		parentTransform = GetComponent<Transform> ();
	}

	// Update is called once per frame
	void Update () {
		
		// Always look at center of map
		transform.LookAt (CenterCube);

		// WASD for moving Horizontally and Vertically
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 5.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 5.0f;
		parentTransform.Translate (x, 0f, z);

		// Spacebar for ascending
		if (Input.GetKey(KeyCode.Space)) {
			parentTransform.Translate (0f, 5.0f * Time.deltaTime, 0f);
		}
	}
}
