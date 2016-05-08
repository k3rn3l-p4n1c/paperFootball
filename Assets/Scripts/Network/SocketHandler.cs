using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using Abrio;
using System.Net;
using System.Collections.Generic;


public class SocketHandler {

	const string HOST = "127.0.0.1";

	OutterWorldState worldState;
	GameLogic gameLogic;
	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[8142];

	public SocketHandler(OutterWorldState ws, GameLogic gl){
		worldState = ws;
        SetupServer ();
		gameLogic = gl;
		Debug.Log (ws.ToString ());
	}

  

	private void SetupServer()
	{
		byte[] bytes = new byte[1024];
		try
		{
			_clientSocket.Connect(new IPEndPoint(IPAddress.Parse(HOST), 8000));
			int numberOfByte = _clientSocket.Receive(_recieveBuffer);
			bytes = new byte[numberOfByte];
			Buffer.BlockCopy(_recieveBuffer, 0, bytes, 0, numberOfByte);

			if(Response.Deserialize(bytes).Type == "3")
				Debug.Log("Connected successfully");

		}
		catch (SocketException ex)
		{
			Debug.Log(ex.Message);
		}

		AuthEvent authEvent = new AuthEvent();
		authEvent.DeviceId = "koosha";
		authEvent.UserId = "bardia";
		authEvent.PrivateKey = "091973";

		SendData(AuthEvent.SerializeToBytes(authEvent));


		int n = _clientSocket.Receive(_recieveBuffer);
		bytes = new byte[n];
		Buffer.BlockCopy(_recieveBuffer, 0, bytes, 0, n);
		if(Response.Deserialize(bytes).Type == "6")
			Debug.Log("Authenticated succesfully");
        
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}

	public void Send(string body){
		EventWrapper newEvent = new EventWrapper ();
		BasicEvent basicEvent = new BasicEvent ();
		basicEvent.Title = "Ball";
		basicEvent.Body = body;
		newEvent.eventType = EventWrapper.EventType.BasicEvent;
		newEvent.BasicEvent = basicEvent;
		SendData (EventWrapper.SerializeToBytes (newEvent));
	}

	private void SendData(byte[] data)
	{
		_clientSocket.Send(data);
	}

	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);

		Debug.Log ("SUCKIT");
		if (recieved <= 0)
			return;

		//Copy the recieved data into new buffer , to avoid null bytes
        //now  all Data is in recData
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);
        
		BasicEvent incommingEvent = EventWrapper.Deserialize (recData).BasicEvent;
		string[] serverData = incommingEvent.Body.Split(' ');
		string head = serverData [0].Trim ();
		Debug.Log (incommingEvent.Body);
		Debug.Log (incommingEvent.Title);
		Debug.Log (head);
		Debug.Log (worldState.ToString ());
		switch(head) {
		case "Ball":
			Debug.Log ("Ball pos received: "+serverData.Length.ToString());
			worldState.update (
				new Vector3 (Int32.Parse (serverData [1]), Int32.Parse (serverData [2])),
				new Vector3 (Int32.Parse (serverData [3]), Int32.Parse (serverData [4])),
				new Vector3 (Int32.Parse (serverData [5]), Int32.Parse (serverData [6])),
				new Vector3 (Int32.Parse (serverData [7]), Int32.Parse (serverData [8])),
				new Vector3 (Int32.Parse (serverData [9]), Int32.Parse (serverData [10])),
				new Vector3 (Int32.Parse (serverData [11]), Int32.Parse (serverData [12])));

			Debug.Log ("Ball pos finished");
			break;
		case "Turn":
			Debug.Log ("TURRRRN");
			worldState.turn = Int32.Parse (serverData [1]);
			gameLogic.StateMachine.SetTurn (worldState.turn);
			break;
		}

		//Start receiving again
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}

    public void notify(float newX, float newY, int ballIndex)
    {

    }
}
