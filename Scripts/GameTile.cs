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

	// Helper for selecting terrain appearence
	public void ChangeMaterial()
	{
		Renderer rend = GetComponent<Renderer> ();
		rend.material.shader = Shader.Find ("Specular");

		if (IsSelected) {
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
