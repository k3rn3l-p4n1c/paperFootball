using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using Abrio;
using System.Net;
using System.Collections.Generic;
using System.Threading;


public class SocketHandler {

	const string HOST = "127.0.0.1";

	OutterWorldState worldState;
	GameLogic gameLogic;
	bool connected = false;
	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer;
	[ThreadStatic]
	private int retryDelay = 1;

	private Thread thread;

	private object mutex = new object();

	public SocketHandler(OutterWorldState ws, GameLogic gl){
		worldState = ws;
		gameLogic = gl;
		Debug.Log (ws.ToString ());
		ThreadStart ts = new ThreadStart (this.SetupServer);
		thread = new Thread (ts);
		thread.Start ();
	}

	public void Close(){
		thread.Abort();
		_clientSocket.Close ();
	}

	public void SetupServer()
	{
		byte[] bytes = new byte[1024];
		while (true) {
			try {
				lock (mutex) {
					if (connected) {
						Debug.Log ("Ignored");
						return;
					}
					Debug.Log ("Try after " + retryDelay.ToString () + " seconds");
					Thread.Sleep (retryDelay * 1000);

					Debug.Log ("Try to connect");
					if (connected) {
						Debug.Log ("Ignored");
						return;
					}
					retryDelay *= 2;
					_clientSocket.Connect (new IPEndPoint (IPAddress.Parse (HOST), 8000));
					Debug.Log ("Connect");
					_recieveBuffer = new byte[8142];
					int numberOfByte = _clientSocket.Receive (_recieveBuffer);
					bytes = new byte[numberOfByte];
					Buffer.BlockCopy (_recieveBuffer, 0, bytes, 0, numberOfByte);

					if (Response.Deserialize (bytes).Type == "3")
						Debug.Log ("Connected successfully");

					AuthEvent authEvent = new AuthEvent ();
					authEvent.DeviceId = "koosha";
					authEvent.UserId = "bardia";
					authEvent.PrivateKey = "0212526";

					TrySendData (AuthEvent.SerializeToBytes (authEvent));

					int n = _clientSocket.Receive (_recieveBuffer);
					bytes = new byte[n-2];
					Buffer.BlockCopy (_recieveBuffer, 2, bytes, 0, n-2);
					if (Response.Deserialize (bytes).Type == "6")
						Debug.Log ("Authenticated succesfully");

					connected = true;
					_clientSocket.BeginReceive (_recieveBuffer, 0, 2, SocketFlags.None, new AsyncCallback (ReceiveFrameCallback), null);
					retryDelay = 1;
					return;
				}
			} catch (SocketException ex) {
				Debug.Log ("Fail to connect: " + ex.Message + ex.StackTrace);
				connected = false;
				_clientSocket.Close ();
				_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			}
		}
	}

	public void Send(string title,string body){
		if (connected) {
			EventWrapper newEvent = new EventWrapper ();
			BasicEvent basicEvent = new BasicEvent ();
			basicEvent.Title = title;
			basicEvent.Body = body;
			newEvent.eventType = EventWrapper.EventType.BasicEvent;
			newEvent.BasicEvent = basicEvent;
			TrySendData (EventWrapper.SerializeToBytes (newEvent));
		}
	}

	private bool TrySendData(byte[] data)
	{
		try {		
			int len = data.Length;
			byte[] data_with_header = new byte[len + 2];
			data_with_header[0] = (byte)(len / 0x00FF);
			data_with_header[1] = (byte)(len % 0x00FF);	
			data.CopyTo (data_with_header, 2);	

			int sent_bytes = _clientSocket.Send (data_with_header);
			if(sent_bytes!= data_with_header.Length)
				Debug.Log("Except to send "+data_with_header.Length.ToString()+" but send "+  sent_bytes.ToString());
			return true;
		} catch (Exception e) {
			Debug.Log ("Error in TrySendData "+e.StackTrace);
			return false;
		}
	}
	private void ReceiveFrameCallback(IAsyncResult AR){
		int recieved = _clientSocket.EndReceive(AR);

		if (recieved != 2)
			return;

		int size = ((int)(_recieveBuffer [0]) << 4) + (int)(_recieveBuffer [1]);
		_clientSocket.BeginReceive(_recieveBuffer, 0, size, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}
	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);

		if (recieved <= 0)
			return;

		//Copy the recieved data into new buffer , to avoid null bytes
        //now  all Data is in recData
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);
        
		BasicEvent incommingEvent = EventWrapper.Deserialize (recData).BasicEvent;
		string[] serverData = incommingEvent.Body.Split(' ');
		string head = serverData [0].Trim ();
		Debug.Log (incommingEvent.Title + " - " + worldState.turn);
		Debug.Log (head);
		Debug.Log (worldState.ToString ());
		switch(head) {
		case "Ball":
			worldState.update (
				new Vector3 (-float.Parse (serverData [1]), -float.Parse (serverData [2])),
				new Vector3 (-float.Parse (serverData [3]), -float.Parse (serverData [4])),
				new Vector3 (-float.Parse (serverData [5]), -float.Parse (serverData [6])),
				new Vector3 (-float.Parse (serverData [7]), -float.Parse (serverData [8])),
				new Vector3 (-float.Parse (serverData [9]), -float.Parse (serverData [10])),
				new Vector3 (-float.Parse (serverData [11]),-float.Parse (serverData [12])));
			break;
		case "Turn":
			Debug.Log ("TURRRRN");
			worldState.turn = Int32.Parse (serverData [1]);
			gameLogic.StateMachine.SetTurn (worldState.turn);
			break;
		case "ChTurn":
			gameLogic.StateMachine.YourTurn ();
			break;
		}

		//Start receiving again
		_clientSocket.BeginReceive (_recieveBuffer, 0, 2, SocketFlags.None, new AsyncCallback (ReceiveFrameCallback), null);
	}

    public void notify(float newX, float newY, int ballIndex)
    {

    }
}
