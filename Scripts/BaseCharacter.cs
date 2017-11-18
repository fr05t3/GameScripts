using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour {
	// Fields
	public int HitPoints = 100;
	public int MoveSpeed = 30;
	public Vector3 Coordinates;
	public GameController ControllerInstance;
	public GameTile TileOccupied;
	public List<GameTile> MoveableTiles = new List<GameTile> ();

	Color startColor;
	bool isHighlighted;

	// Returns true if tile is within range, and not occupied
	public bool IsTileMoveable(GameTile tile)
	{
		bool isInRange = Mathf.Abs ((float)tile.Coordinates.x - (float)Coordinates.x) <=  GetMoveThreshhold () 
			&& Mathf.Abs ((float)tile.Coordinates.z - (float)Coordinates.z)  <= GetMoveThreshhold ();
		return isInRange;// && !tile.IsOccupied;
	}

	//  Returns true if character moves to tile, false otherwise
	public bool MoveToTile(GameTile tile)
	{
		bool willMove = IsTileMoveable (tile);

		if (willMove) {
			// Get tile 'floor'
			float tileMidHeight = tile.GetComponent<MeshFilter> ().sharedMesh.bounds.extents.y;
			Vector3 tilePosition = tile.transform.position;
			Vector3 tileFloor = new Vector3 (tilePosition.x, tilePosition.y + tileMidHeight, tilePosition.z);

			// Move character 'feet' to tile 'floor'
			float characterMidHeight = GetComponent<MeshFilter> ().sharedMesh.bounds.extents.y;
			Vector3 characterCenter = GetComponent<MeshFilter> ().sharedMesh.bounds.center;
			transform.position = new Vector3 (tileFloor.x, tileFloor.y + characterMidHeight, tileFloor.z);

			// Changed occupied tile to current tile
			TileOccupied.IsOccupied = false;
			tile.IsOccupied = true;
			TileOccupied = tile;

			// Change current Coordinates
			Coordinates = tile.Coordinates;
		}

		return willMove;
	}

	public int GetMoveThreshhold(){
		int tileWidth = 5;
		return (MoveSpeed / tileWidth);
	}

	public void ResetMoveableTiles() 
	{
		foreach (GameTile tile in MoveableTiles) {
			tile.IsSelected = false;
			tile.ChangeMaterial ();
		}
	}

	void Start()
	{
		Renderer parentRenderer = GetComponent<Renderer> ();
		startColor = parentRenderer.material.color;
	}
		
	void OnMouseDown()
	{
		ControllerInstance.SelectedCharacter = this;
	}
		
	void OnMouseEnter()
	{
		isHighlighted = true;
	}

	void OnMouseExit()
	{
		isHighlighted = false;
	}

	void Update()
	{
		ChooseColor();
	}

	void ChooseColor() 
	{
		Renderer rend = GetComponent<Renderer> ();
		rend.material.shader = Shader.Find ("Specular");

		if (ControllerInstance.SelectedCharacter == this) {
			rend.material.SetColor ("_Color", Color.cyan);
		} else if (isHighlighted) {
			rend.material.SetColor ("_Color", Color.black);
		} else {
			rend.material.SetColor ("_Color", startColor);
		}
	}
}
