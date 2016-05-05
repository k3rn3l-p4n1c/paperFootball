using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using Abrio;
using System.Net;
using System.Collections.Generic;


public class SocketHandler {

	const string HOST = "172.17.10.6";

    GameLogic logic;
	private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	private byte[] _recieveBuffer = new byte[8142];

	public SocketHandler(GameLogic gl){
        logic = gl;
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
        Debug.Log("in async:");
		//Check how much bytes are recieved and call EndRecieve to finalize handshake
		int recieved = _clientSocket.EndReceive(AR);

		if (recieved <= 0)
			return;

		//Copy the recieved data into new buffer , to avoid null bytes
        //now  all Data is in recData
		byte[] recData = new byte[recieved];
		Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

        //parsing
        float newX, newY,newVelocityX,newVelocityY;
        int ballIndex;
        Debug.Log("Server Info title:" + BasicEvent.Deserialize(recData).Title);
        Debug.Log("Server Info Body:" + BasicEvent.Deserialize(recData).Body);
        string[] serverData = BasicEvent.Deserialize(recData).Body.Split(' ');
        for (int i = 0; i < serverData[0].Length; i++)
        {
            Debug.Log(serverData[0].ToCharArray()[i]);
        }
            Debug.Log("Server Info data 0 :" + serverData[0].Length.ToString());
        Debug.Log("S: " + (serverData[0].Equals("  server  Ball")));
        if (serverData[0].Equals("serverData"))
        {
            ballIndex = Int32.Parse(serverData[1]);
            newX = Int32.Parse(serverData[2]);
            newY = Int32.Parse(serverData[3]);
            newVelocityX = Int32.Parse(serverData[4]);
            newVelocityY = Int32.Parse(serverData[5]);
            Vector3 veloVec = new Vector3(newVelocityX, newVelocityY);
            Vector3 posVec = new Vector3(newX, newY);
            logic.moveOpponent(posVec, ballIndex, veloVec);
        }
        switch (serverData[0])
        {
            case "  server  Ball":
                Debug.Log("in case");
                ballIndex = Int32.Parse(serverData[1]);
                newX = Int32.Parse(serverData[2]);
                newY = Int32.Parse(serverData[3]);
                newVelocityX = Int32.Parse(serverData[4]);
                newVelocityY = Int32.Parse(serverData[5]);
                Vector3 veloVec = new Vector3(newVelocityX, newVelocityY);
                Vector3 posVec = new Vector3(newX, newY);
                logic.moveOpponent(posVec, ballIndex, veloVec);

                break;

        }

		//Start receiving again
		_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
	}

    public void notify(float newX, float newY, int ballIndex)
    {

    }
}
