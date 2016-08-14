using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {

	protected VirtualTile data;
	Behaviour halo;

	protected void ApplyColors ()
	{
		transform.FindChild ("Cube11").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE11);
		transform.FindChild ("Cube12").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE12);
		transform.FindChild ("Cube13").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE13);
		transform.FindChild ("Cube14").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE14);
		transform.FindChild ("Cube21").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE21);
		transform.FindChild ("Cube22").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE22);
		transform.FindChild ("Cube23").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE23);
		transform.FindChild ("Cube24").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE24);
		transform.FindChild ("Cube31").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE31);
		transform.FindChild ("Cube32").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE32);
		transform.FindChild ("Cube33").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE33);
		transform.FindChild ("Cube34").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE34);
		transform.FindChild ("Cube41").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE41);
		transform.FindChild ("Cube42").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE42);
		transform.FindChild ("Cube43").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE43);
		transform.FindChild ("Cube44").GetComponent<Renderer> ().material.color = data.colorAt (VirtualTile.SQUARE44);
	}

	// Use this for initialization
	void Start () {
		Material m = new Material(Shader.Find("Transparent/Diffuse"));
		m.color = VirtualTile.colorless;

		foreach (Transform child in GetComponentInChildren<Transform>()) {
			child.GetComponent<Renderer>().material = m;
		}
			
		data = new VirtualTile (0, 0, 0);
		ApplyColors ();
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
}
