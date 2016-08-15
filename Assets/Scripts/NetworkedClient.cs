using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.Experimental.Networking ;

public class NetworkedClient : MonoBehaviour {

	public static NetworkedClient Instance = null;

	public int hostSocket = 3000;
	public string hostIpAddress = "127.0.0.1";
    private bool isActivelyPolling = true;


	void Start()
	{
		Instance = this;

        StartCoroutine(FetchGameState());
	}


    IEnumerator FetchGameState()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://" + hostIpAddress + ":" + hostSocket + "/games/1");
        yield return www.Send();

        isActivelyPolling = false;
        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

                GameEvent myEvent = GameEvent.fromJson(Encoding.UTF8.GetString(results));

                Debug.Log("CLIENT incoming message event received: " + myEvent);
                HandleIncomingEvent(myEvent);
            }
    }

    IEnumerator FetchAfter(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);

        StartCoroutine(FetchGameState());
    }
		
	public void SendMessage(GameEvent eventToSend)
	{
		//SendMessage(eventToSend, myConnectionInfo.hostId, myConnectionInfo.connectionId, myConnectionInfo.channelId);       
	}

	private void SendMessage(GameEvent eventToSend, int host, int connection, int channel)
	{
		Debug.Log("Sending Message: " + eventToSend + " to " + host + ":" + connection + ":" + channel);

		byte error;
		byte[] buffer = Encoding.UTF8.GetBytes(eventToSend.asJson());

		NetworkTransport.Send(host, connection, channel, buffer, buffer.Length, out error);

		LogNetworkError(error);
	}

	
	// Update is called once per frame
	void Update()
	{
        if (!isActivelyPolling)
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
}
