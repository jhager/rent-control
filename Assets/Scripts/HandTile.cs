using UnityEngine;
using System.Collections;

/**
 * Represents a tile in the hand.  It acts like a tile on the board
 * except that it has a primary color + can be merged with the board.  
 */
public class HandTile : GameTile {

	public bool initalized = false;
	public float respawnAfter = 2.0f;

    GameObject indicator;
    TextMesh indicatorText;

    public void init() {

        indicator = GameObject.Find("RotateIndicator");
		if (indicator != null) {
			indicatorText = indicator.GetComponent<TextMesh> ();
		}

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

        //todo: figure out rotatin from card transform
        Vector3 vec = transform.eulerAngles;
        float currentY = vec.y;
        float snapY = Mathf.Round(vec.y / 90) * 90;
		if (indicatorText != null) {
			indicatorText.text = vec.y.ToString ("n") + ", " + snapY.ToString ();
		}

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger with " + gameObject.name + " and " + other.gameObject.name);

        if (other.gameObject.name.Contains("CenterGameBoard") ) { 
            //todo: figure out rotatin from card transform
            Vector3 vec = transform.eulerAngles;
            vec.y = Mathf.Round(vec.y / 90) * 90;
       
            //Debug.LogFormat("Rotation rounded y:{0}, current y:{1}, /90:{2}", vec.y, transform.eulerAngles.y, vec.y / 90 );

            if (vec.y == 0 || vec.y == 360)
            {
                data = data.reoriented(VirtualTile.Orientation.Up);
            }
            else if (vec.y == 90)
            {
                data = data.reoriented(VirtualTile.Orientation.Clockwise90);
            }
            else if (vec.y == 180)
            {
                data = data.reoriented(VirtualTile.Orientation.UpsideDown);
            }
            else if (vec.y == 270)
            {
                data = data.reoriented(VirtualTile.Orientation.CounterClockwise90);
            }
        
            MergeWithBoard(other.gameObject.GetComponent<GameTile>());
        }
    }

    protected void ApplyColors()
    {
        updateLocation("Cube11", VirtualTile.SQUARE11);
        updateLocation("Cube12", VirtualTile.SQUARE12);
        updateLocation("Cube13", VirtualTile.SQUARE13);
        updateLocation("Cube14", VirtualTile.SQUARE14);

        updateLocation("Cube21", VirtualTile.SQUARE21);
        updateLocation("Cube22", VirtualTile.SQUARE22);
        updateLocation("Cube23", VirtualTile.SQUARE23);
        updateLocation("Cube24", VirtualTile.SQUARE24);

        updateLocation("Cube31", VirtualTile.SQUARE31);
        updateLocation("Cube32", VirtualTile.SQUARE32);
        updateLocation("Cube33", VirtualTile.SQUARE33);
        updateLocation("Cube34", VirtualTile.SQUARE34);

        updateLocation("Cube41", VirtualTile.SQUARE41);
        updateLocation("Cube42", VirtualTile.SQUARE42);
        updateLocation("Cube43", VirtualTile.SQUARE43);
        updateLocation("Cube44", VirtualTile.SQUARE44);
    }

    private void updateLocation(string cubeName, ushort location)
    {
        //get the relevant cube
        GameObject cube = transform.FindChild(cubeName).gameObject;

        Color d = data.colorAt(location);
        if( d == VirtualTile.colorless) d.a = 0.4f;
        //set the color
        cube.GetComponent<Renderer>().material.color = d;
    }
}
