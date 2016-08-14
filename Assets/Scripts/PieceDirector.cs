using UnityEngine;
using System.Collections;

public class PieceDirector : MonoBehaviour {

	public GameTile centerBoard1;
	public GameTile centerBoard2;

	public HandTile activePiece;

	public HandTile[] allPlayerPieces = new HandTile[3];

	// Use this for initialization
	void Start () {
		allPlayerPieces [0] = (HandTile) GameObject.Find ("Player1HandTile1").GetComponent<HandTile>();
		allPlayerPieces [1] = (HandTile) GameObject.Find ("Player1HandTile2").GetComponent<HandTile>();
		allPlayerPieces [2] = (HandTile) GameObject.Find ("Player1HandTile3").GetComponent<HandTile>();

		activePiece = allPlayerPieces [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			activePiece.SetActive (false);
			activePiece = allPlayerPieces [0];
			activePiece.SetActive (true);
		} else if (Input.GetKeyDown (KeyCode.S)) {
			activePiece.SetActive (false);
			activePiece = allPlayerPieces [1];
			activePiece.SetActive (true);
		} else if (Input.GetKeyDown (KeyCode.D)) {
			activePiece.SetActive (false);
			activePiece = allPlayerPieces [2];
			activePiece.SetActive (true);
		}

		if (activePiece == null) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			activePiece.MergeWithBoard (centerBoard1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) ) {
			activePiece.MergeWithBoard (centerBoard2);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			activePiece.Reorient ( VirtualTile.Orientation.CounterClockwise90);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			activePiece.Reorient(VirtualTile.Orientation.Clockwise90);
		}
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow)) {
			activePiece.Reorient (VirtualTile.Orientation.UpsideDown);
		}
	}
}
