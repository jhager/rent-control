using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.Networking;



/**
 * Acts as the host for the game.  Uses LLAPI, since that 
 * is what I understand / know best.  
 */
public class NetworkHost : MonoBehaviour {

	public static NetworkHost Instance = null;


	private class ConnectionInfo
	{
		public int hostId;
		public int connectionId;
		public int channelId;

		public override bool Equals(object obj)
		{
			return hostId == ((ConnectionInfo) obj).hostId && 
				connectionId == ((ConnectionInfo) obj).connectionId &&
				channelId == ((ConnectionInfo) obj).channelId;
		}

		public override int GetHashCode()
		{
			return hostId + 37 * connectionId;
		}
	};

	public int socketPort = 12765;
	int mServerSocket = -1;
	List<ConnectionInfo> clientConnections = new List<ConnectionInfo>();
	HashSet<string> serverProcessedEvents = new HashSet<string>();


	// Use this for initialization
	void Awake () {
		Debug.Log("NetworkHost.Awake()");

		Instance = this;

		NetworkTransport.Init();

		ConnectionConfig config = new ConnectionConfig();
		config.AddChannel(QosType.Reliable);

		int maxConnections = 10;
		HostTopology topology = new HostTopology(config, maxConnections);

		//this device will listen on the specified socket for clients.       
		mServerSocket = NetworkTransport.AddHost(topology, socketPort);

		Debug.Log("Socket Open. SocketId is: " + mServerSocket);

	}
	
	// Update is called once per frame
	void Update()
	{
		int recHostId;
		int recConnectionId;
		int recChannelId;
		byte[] recBuffer = new byte[1024];
		int bufferSize = 1024;
		int dataSize;
		byte error;
		NetworkEventType recNetworkEvent;
		do
		{
			recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);

			LogNetworkError(error);

			switch (recNetworkEvent)
			{
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:
				Debug.Log("HOST incoming connection event received " + recHostId + ":" + recConnectionId + ":" + recChannelId);
				ConnectionInfo clientInfo = extractClientConnectionInfo(recHostId, recConnectionId, recChannelId);
				clientConnections.Add(clientInfo);
			
				break;
			case NetworkEventType.DataEvent:
				Debug.Log("HOST incoming data");
				string result = Encoding.UTF8.GetString(recBuffer);
				GameEvent myEvent = GameEvent.fromJson(result);

				Debug.Log("HOST incoming message event received: " + myEvent);
				HandleIncomingEvent(myEvent);

				break;
			case NetworkEventType.DisconnectEvent:
				Debug.Log("HOST remote client event disconnected");
				ConnectionInfo disconnectedClientInfo = extractClientConnectionInfo(recHostId, recConnectionId, recChannelId);
				clientConnections.Remove(disconnectedClientInfo);
				break;
			}

		} while (recNetworkEvent != NetworkEventType.Nothing);


	}

	private static void LogNetworkError(byte error)
	{
		if (error != (byte)NetworkError.Ok)
		{
			NetworkError nerror = (NetworkError)error;
			Debug.Log("Error " + nerror.ToString());
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

	private ConnectionInfo extractClientConnectionInfo(int recHostId, int recConnectionId, int recChannelId)
	{
		ConnectionInfo clientInfo = new ConnectionInfo();
		clientInfo.hostId = recHostId;
		clientInfo.connectionId = recConnectionId;
		clientInfo.channelId = recChannelId;
		return clientInfo;
	}
		
	public void PropogateEvent (GameEvent e)
	{


	}

	private void SendMessage(GameEvent eventToSend, int host, int connection, int channel)
	{
		Debug.Log("HOST Sending Message: " + eventToSend + " to " + host + ":" + connection + ":" + channel);

		byte error;
		byte[] buffer = Encoding.UTF8.GetBytes(eventToSend.asJson());

		NetworkTransport.Send(host, connection, channel, buffer, buffer.Length, out error);

		LogNetworkError(error);
	}

}
