using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PieceState
{
	public string id;
	public ushort red = 0;
	public ushort yellow = 0;
	public ushort blue = 0;

	public PieceState (string id, ushort red, ushort yellow, ushort blue) {
		this.id = id;
		this.red = red;
		this.yellow = yellow;
		this.blue = blue;
	}
}