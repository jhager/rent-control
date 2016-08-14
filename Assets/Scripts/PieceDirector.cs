using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PieceDirector : MonoBehaviour {
	public static int NUMBER_OF_PLAYERS = 3;
	public static int NUMBER_OF_TILE_PER_PLAYER = 3;
	public static int TOTAL_TILES = NUMBER_OF_PLAYERS * NUMBER_OF_TILE_PER_PLAYER;

	public GameTile centerBoard1;
	public GameTile centerBoard2;

	private HandTile activePiece;

	private int currentPlayersTurn = 0;

	private HandTile[] allPlayerPieces = new HandTile[9];
	private Text scoreText;

	// Use this for initialization
	void Start () {
		allPlayerPieces [0] = (HandTile) GameObject.Find ("Player1HandTile1").GetComponent<HandTile>();
		allPlayerPieces [1] = (HandTile) GameObject.Find ("Player1HandTile2").GetComponent<HandTile>();
		allPlayerPieces [2] = (HandTile) GameObject.Find ("Player1HandTile3").GetComponent<HandTile>();
		allPlayerPieces [3] = (HandTile) GameObject.Find ("Player2HandTile1").GetComponent<HandTile>();
		allPlayerPieces [4] = (HandTile) GameObject.Find ("Player2HandTile2").GetComponent<HandTile>();
		allPlayerPieces [5] = (HandTile) GameObject.Find ("Player2HandTile3").GetComponent<HandTile>();
		allPlayerPieces [6] = (HandTile) GameObject.Find ("Player3HandTile1").GetComponent<HandTile>();
		allPlayerPieces [7] = (HandTile) GameObject.Find ("Player3HandTile2").GetComponent<HandTile>();
		allPlayerPieces [8] = (HandTile) GameObject.Find ("Player3HandTile3").GetComponent<HandTile>();

		scoreText = GameObject.Find("Score").GetComponent<Text>();

	}

	public void MergeRequested(HandTile piece, GameTile board, VirtualTile.Orientation orientation) {
		//todo animate merge.
		activePiece.SetActive (false);
		activePiece = piece;
		activePiece.SetActive (true);
		activePiece = null;
	
		if (board.canMergeWith (piece.GetData (), VirtualTile.Orientation.Up)) {
			piece.MergeWithBoard (board);
			currentPlayersTurn++;
			currentPlayersTurn = currentPlayersTurn % NUMBER_OF_PLAYERS;

			RecalculateScore ();
		} else {
			piece.SetActive (false);
		}
	}

	void RecalculateScore() {
		int score = centerBoard1.GetData ().Score () + centerBoard2.GetData ().Score ();
		scoreText.text = "Score: " + score;
	}

	void AdjustActivePiece (int position)
	{
		if (activePiece != null) {
			activePiece.SetActive (false);
		}
		int index = position + currentPlayersTurn * NUMBER_OF_PLAYERS;
		activePiece = allPlayerPieces [index];
		activePiece.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			AdjustActivePiece (0);
		} else if (Input.GetKeyDown (KeyCode.S)) {
			AdjustActivePiece (1);
		} else if (Input.GetKeyDown (KeyCode.D)) {
			AdjustActivePiece (2);
		}

		if (activePiece == null) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			MergeRequested (activePiece, centerBoard1, VirtualTile.Orientation.Up);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) ) {
			MergeRequested (activePiece, centerBoard2, VirtualTile.Orientation.Up);
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
