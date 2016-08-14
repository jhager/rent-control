using UnityEngine;
using System.Collections;

/**
 * Represents a tile in the hand.  It acts like a tile on the board
 * except that it has a primary color + can be merged with the board.  
 */
public class HandTile : GameTile {

	public bool initalized = false;
	public float respawnAfter = 2.0f;

	void init() {
		data = new VirtualTile ();
		ApplyColors ();
	}

	IEnumerator reinit() {
		//clear tile...It has beem merged.
		data = new VirtualTile (0, 0, 0);
		ApplyColors ();

		//wait
		yield return new WaitForSeconds (respawnAfter);
		init ();
	}

	public void MergeWithBoard (GameTile boardToMergeWith)
	{
		if (boardToMergeWith.canMergeWith (data, VirtualTile.Orientation.Up)) {
			boardToMergeWith.mergeWith (data, VirtualTile.Orientation.Up);
			StartCoroutine (reinit ());
		}
	}

	public void Reorient (VirtualTile.Orientation orientation)
	{
		data = data.reoriented (orientation);
		ApplyColors ();
	}

	// Update is called once per frame
	void Update () {
		if (!initalized) {
			init ();
			initalized = true;
		}
			
	}
}
