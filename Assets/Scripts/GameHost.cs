using UnityEngine;
using System.Collections;

public class GameHost : MonoBehaviour {

	public PieceDirector gameDirector;
	public NetworkHost neworkHost;

	// Use this for initialization
	void Awake () {
		neworkHost.OnIncomingEvent += OnIncomingEvent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator SendCurrentState(float after) {
		yield return new WaitForSeconds (after);

		if (gameDirector != null) {
			NetworkHost.Instance.PropogateEvent (new GameEvent (gameDirector.GetGameState ()));
		}
	}

	private void OnIncomingEvent(object sender, GameEvent e)
	{
		//todo
	}
}
