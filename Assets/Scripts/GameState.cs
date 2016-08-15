using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
	public List<PieceState> boards = new List<PieceState>();
	public List<PieceState> pieces = new List<PieceState>();
	public int turn = 0;
}