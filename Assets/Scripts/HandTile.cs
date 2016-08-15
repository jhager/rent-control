using UnityEngine;
using System.Collections;

/**
 * Represents a tile in the hand.  It acts like a tile on the board
 * except that it has a primary color + can be merged with the board.  
 */
public class HandTile : GameTile {

	public bool initalized = false;
	public float respawnAfter = 2.0f;

	public void init() {
		data = new VirtualTile ();
		ApplyColors ();
	}

	 IEnumerator reinit(float waitTime) {
		//clear tile...It has beem merged.
		data = new VirtualTile (0, 0, 0);
		ApplyColors ();
		SetActive (false);

		//wait
		yield return new WaitForSeconds (waitTime);
		init ();
	}

	public void MergeWithBoard (GameTile boardToMergeWith, VirtualTile.Orientation orientation)
	{
		if (boardToMergeWith.canMergeWith (data, orientation)) {
			boardToMergeWith.mergeWith (data, orientation);
			StartCoroutine (reinit (respawnAfter));
		}
	}

	public void MergeWithBoard (GameTile boardToMergeWith)
	{
		VirtualTile.Orientation orientation = VirtualTile.Orientation.Up;
		MergeWithBoard (boardToMergeWith, orientation);
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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with " + gameObject.name + " and " + other.gameObject.name);
        //todo: figure out rotatin from card transform

        MergeWithBoard(other.gameObject.GetComponent<GameTile>());
    }
}
