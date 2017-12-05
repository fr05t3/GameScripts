using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour {
	// Shared Enum for land types
	public enum TerrainTypes {FlatLand, Water};

	public TerrainTypes Terrain = TerrainTypes.FlatLand;
	public Vector3 Coordinates;
	public bool IsOccupied;
	public bool IsSelected;
	public GameController ControllerInstance;

	bool isMouseOver = false;

	public void ChangeTerrain (TerrainTypes type)
	{
		Terrain = type;
		ChangeMaterial ();
	}

	void Start() 
	{
		ChangeMaterial ();
	}

	void OnMouseDown()
	{
		ControllerInstance.SelectedTile = this;
		ControllerInstance.TryMove ();
	}

	void OnMouseEnter()
	{
		isMouseOver = true;
		ChangeMaterial ();
	}

	void OnMouseExit()
	{
		isMouseOver = false;
		ChangeMaterial ();
	}

	// Helper for selecting terrain appearence
	public void ChangeMaterial()
	{
		Renderer rend = GetComponent<Renderer> ();
		rend.material.shader = Shader.Find ("Specular");

		if (isMouseOver){
			rend.material.SetColor ("_Color", Color.black);
		} else if (IsSelected) {
			rend.material.SetColor ("_Color", Color.red);
		} else if (Terrain == TerrainTypes.FlatLand) {
			rend.material.SetColor ("_Color", Color.yellow);
		} else if (Terrain == TerrainTypes.Water) {
			rend.material.SetColor ("_Color", Color.blue);
		} else {
			Debug.LogError ("Tile is invalid Terrain Type"); // Just don't be stupid ok?
		}
	}
}
