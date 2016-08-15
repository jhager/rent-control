using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class GameEvent :EventArgs {

    public int id;
	public string lastEventId;
	public GameState gameState;

	public GameEvent(GameState gameState) {
        this.id = 1;
		this.lastEventId = Guid.NewGuid().ToString();
		this.gameState = gameState;
	}


	public string asJson()
	{
		return JsonUtility.ToJson(this);
	}

	public static GameEvent fromJson(string json)
	{
		return JsonUtility.FromJson<GameEvent>(json);
	}

	public override string ToString()
	{
		return asJson();
	}
}
