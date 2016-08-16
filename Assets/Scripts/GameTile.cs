using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {

	protected VirtualTile data;
	Behaviour halo;
    
	protected void ApplyColors ()
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

    private void updateLocation( string cubeName,  ushort location )
    {
        //get the relevant cube
        GameObject cube = transform.FindChild(cubeName).gameObject;

        //set the color
        cube.GetComponent<Renderer>().material.color = data.colorAt( location );

        //activate the relevant prefab
        string cubeTennant = "Tennant" + data.tennantAt( location );

        for (int i = 0; i < cube.transform.childCount; i++)
        {
            GameObject t = cube.transform.GetChild(i).gameObject;
            t.SetActive( t.name == cubeTennant ? true : false);           
        }
    }

    public void init() {
		data = new VirtualTile (0, 0, 0);
		ApplyColors ();
	}

	// Use this for initialization
	void Start () {
			
		init ();
		halo = (Behaviour)GetComponent ("Halo");
		SetActive (false);
	}

	public void SetActive(bool active) {
		if (halo != null) {
			halo.enabled = active;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void mergeWith(VirtualTile v, VirtualTile.Orientation orientation) {
		data.merge (v, orientation);
		ApplyColors ();
	}

	public bool canMergeWith(VirtualTile v, VirtualTile.Orientation orientation) {
		return data.canMerge (v, orientation);
	}

	public VirtualTile GetData() {
		return data;
	}

    public void SetPieceState(PieceState data)
    {
        this.data.SetPieceState(data);
        ApplyColors();
    }
}
