using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOnMouseover : MonoBehaviour {

	Renderer parentRenderer;
	Color startColor;

	void Start()
	{
		parentRenderer = GetComponent<Renderer> ();
	}

	void OnMouseEnter()
	{
		startColor = parentRenderer.material.color;
		parentRenderer.material.color = Color.black;
	}

	void OnMouseExit()
	{
		parentRenderer.material.color = startColor;
	}
}
