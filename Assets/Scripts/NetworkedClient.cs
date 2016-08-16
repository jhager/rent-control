using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.Experimental.Networking ;

using System.Net;
using System.IO;
using System.Text;

public class NetworkedClient : MonoBehaviour {

	public static NetworkedClient Instance = null;

	public int hostSocket = 3000;
	public string hostIpAddress = "127.0.0.1";
    private bool isActivelyPolling = true;
	private bool pausePolling = false;


	void Start()
	{
		Instance = this;

        StartCoroutine(FetchGameState());
	}

	void HandleResponse (UnityWebRequest www)
	{
		if (www.isError) {
			Debug.Log (www.error);
		}
		else {
			// Show results as text
			Debug.Log (www.downloadHandler.text);
			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data;
			GameEvent myEvent = GameEvent.fromJson (Encoding.UTF8.GetString (results));
			Debug.Log ("CLIENT incoming message event received: " + myEvent);
			HandleIncomingEvent (myEvent);
		}
	}

    IEnumerator FetchGameState()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://" + hostIpAddress + ":" + hostSocket + "/games/1");
        yield return www.Send();

        isActivelyPolling = false;
        HandleResponse (www);
    }

    IEnumerator FetchAfter(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);

		if (pausePolling) {
			isActivelyPolling = false;
		} else {
			StartCoroutine (FetchGameState ());
		}
    }
		
	public IEnumerator SetGameState(GameEvent gameState) 
	{
		pausePolling = true;

		Debug.Log (gameState.asJson ());
		//application/json; charset=utf-8

		UnityWebRequest wr = new UnityWebRequest("http://" + hostIpAddress + ":" + hostSocket + "/games");
		byte[] buffer = Encoding.UTF8.GetBytes(gameState.asJson());
		UploadHandler uploader = new UploadHandlerRaw(buffer);

		// Will send header: "Content-Type: custom/content-type";
		uploader.contentType = "application/json; charset=utf-8";

		wr.method = "POST";
		wr.uploadHandler = uploader;

		DownloadHandler downloadHandler = new DownloadHandlerBuffer ();
		wr.downloadHandler = downloadHandler;

		yield return wr.Send();

		HandleResponse (wr);
		pausePolling = false;
	}

	
	// Update is called once per frame
	void Update()
	{
        if (!isActivelyPolling && !pausePolling)
        {
            isActivelyPolling = true;
            StartCoroutine(FetchAfter(1.0f));
        }
			
	}


	protected virtual void HandleIncomingEvent(GameEvent e)
	{
		EventHandler<GameEvent> handler = OnIncomingEvent;
		if (handler != null)
		{
			handler(this, e);
		}
	}

	public event EventHandler<GameEvent> OnIncomingEvent;

	private static void LogNetworkError(byte error)
	{
		if (error != (byte)NetworkError.Ok)
		{
			NetworkError nerror = (NetworkError)error;
			Debug.Log("Error " + nerror.ToString());
		}
	}

	//hopefully this works.  or we can figure out how to make the standard POST work.  
	public IEnumerator SendGameSate2 (GameEvent gameState) {
		pausePolling = true;

		string url = "http://" + hostIpAddress + ":" + hostSocket + "/games";

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (new Uri(url));
		request.ContentType = "application/json; charset=utf-8";

		request.Method = "POST";

		ASCIIEncoding encoding = new ASCIIEncoding ();
		byte[] byte1 = encoding.GetBytes (gameState.asJson());
		request.ContentLength = byte1.Length;
		Stream newStream = request.GetRequestStream ();
		yield return new WaitForSeconds(0.001f);
		newStream.Write (byte1, 0, byte1.Length);
		newStream.Close ();

		using (WebResponse response = request.GetResponse ()) {
			using (Stream stream = response.GetResponseStream ()) {
				//process the response
				pausePolling = false;
			}
		}
	}
}
