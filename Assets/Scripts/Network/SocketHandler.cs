using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using Abrio;
using System.Net;
using System.Collections.Generic;


public class SocketHandler {
	const string HOST = "127.0.0.1";

	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[8142];

	public SocketHandler(){
		SetupServer ();
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

	private void SendData(byte[] data)
	{
		_clientSocket.Send(data);
	}

	private void ReceiveCallback(IAsyncResult AR)
	{
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);

		if (recieved <= 0)
			return;

		//Copy the recieved data into new buffer , to avoid null bytes
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

		//Start receiving again
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}
}
