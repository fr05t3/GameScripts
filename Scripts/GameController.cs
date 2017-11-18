using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject CenterCube;

	public GameObject Tile;
	public int BoardLength = 10; 
	public int BoardWidth = 10; 
	public GameObject[,] Board; 

	public GameObject MainCharacter;

	public BaseCharacter SelectedCharacter;
	public GameTile SelectedTile;

	// For checking if new character has been selected
	BaseCharacter lastSelectedCharacter;

	// When a tile is clicked, will try to move selected character to it
	public void TryMove(){
		if (SelectedCharacter && SelectedTile) {
			MoveCharacterToTile (SelectedCharacter.gameObject, SelectedTile.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		// Place center cube in correct location
		CenterCube.transform.position = new Vector3((float)(BoardLength / 2), 0f, (float)(BoardWidth / 2));

		// Create board size and fill with tiles
		Board = new GameObject[BoardWidth, BoardLength];
		for (int i = 0; i < BoardWidth; i++) {
			for (int j = 0; j < BoardLength; j++) {
				Vector3 tileLocation = new Vector3 ((float)i, 0f, (float)j);
				GameObject tile = GameObject.Instantiate (Tile, tileLocation, Quaternion.identity);
				tile.GetComponent<GameTile> ().Coordinates = tileLocation;
				tile.GetComponent<GameTile> ().ControllerInstance = this;
				Board [i, j] = tile;
			}
		}

		// Spawn PC
		BaseCharacter pc = GameObject.Instantiate (MainCharacter).GetComponent<BaseCharacter> ();
		pc.ControllerInstance = this;
		pc.TileOccupied = Board [0, 0].GetComponent<GameTile>();
		MoveCharacterToTile (pc.gameObject, Board [0, 0]);
		pc.ResetMoveableTiles ();
	}

	void Update(){
		if (SelectedCharacter != null) {
			HighlightMoveableTiles (SelectedCharacter);
		}
	}

	bool IsCharacterNew()
	{
		return (SelectedCharacter != null && lastSelectedCharacter != null && lastSelectedCharacter != SelectedCharacter);
	}
		
	void MoveCharacterToTile(GameObject character, GameObject tile) 
	{
		GameTile tileInstance = tile.GetComponent<GameTile>();
		BaseCharacter baseCharacter = character.GetComponent<BaseCharacter>();

		// Move character to tile
		if (!baseCharacter.MoveToTile (tileInstance)) {
			DeselectItems ();
		}

		// Remake moveable tiles list
		baseCharacter.ResetMoveableTiles();
		baseCharacter.MoveableTiles.Clear ();
		int moveThreshhold = baseCharacter.GetMoveThreshhold ();
		// Reindex character moveable tiles
		for (int i = (int)baseCharacter.Coordinates.x - moveThreshhold; i <= (int)baseCharacter.Coordinates.x + moveThreshhold; i++) {
			for (int j = (int)baseCharacter.Coordinates.z - moveThreshhold; j <= (int)baseCharacter.Coordinates.z + moveThreshhold; j++) {
				if(isTileInBounds(i, j) && baseCharacter.IsTileMoveable(tileInstance)){
					GameTile curTile = Board [i, j].GetComponent<GameTile> ();
					curTile.IsSelected = true;
					curTile.ChangeMaterial();
					baseCharacter.MoveableTiles.Add (curTile);
				}
			}
		}
	}

	void HighlightMoveableTiles(BaseCharacter character) 
	{
		foreach (GameTile tile in character.MoveableTiles) {
			tile.ChangeMaterial ();
		}
	}

	bool isTileInBounds(int i, int j)
	{
		return (i >= 0 && j >= 0 && i < BoardLength && j < BoardWidth);
	}

	void DeselectItems()
	{
		SelectedCharacter = null;
		lastSelectedCharacter = null;
		SelectedTile = null;
	}
}
