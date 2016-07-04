using System;
using System.Threading;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

namespace Abrio
{
	public class AbrioClient
	{
		// Constants
//		const string HOST = "198.72.109.14";
		const string HOST = "172.20.10.5";
		const int PORT = 8000;

		[ThreadStatic]
		AbrioState state;
		[ThreadStatic]
		int retryCount;

		private Thread thread;
		private Socket _clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private byte[] buffer;
		private  IAbrioHandler handler;

		public static AbrioClient instance;

		public AbrioClient (IAbrioHandler handler)
		{
			this.handler = handler;
			buffer = new byte[8142];
			state = AbrioState.IDLE;
			retryCount = 0;
			ThreadStart threadStart = new ThreadStart (this.run);
			thread = new Thread (threadStart);

			instance = this;
		}

		public void Start ()
		{
			Debug.Log ("Socket thread start");
			thread.Start ();
		}

		public void Stop ()
		{
			if (thread.IsAlive)
				thread.Abort ();
		}

		private void run ()
		{
			if (state == AbrioState.IDLE)
				connect_server ();
			if (state == AbrioState.CONNECTED)
				_clientSocket.BeginReceive (buffer, 0, 2, SocketFlags.None, new AsyncCallback (ReceiveFrameCallback), null);
		}

		private void connect_server ()
		{
			try {
				state = AbrioState.CONNECTING;
				Debug.Log ("Sleep for " + delayPolicy (retryCount).ToString () + " seconds");
				Thread.Sleep (delayPolicy (retryCount) * 1000);

				Debug.Log ("Try to connect");
				retryCount++;

				_clientSocket.Connect (new IPEndPoint (IPAddress.Parse (HOST), PORT));
				Debug.Log ("Connect");

				int numberOfByte = _clientSocket.Receive (buffer);
				byte[] bytes = new byte[numberOfByte];
				Buffer.BlockCopy (buffer, 0, bytes, 0, numberOfByte);

				if (Response.Deserialize (bytes).Type == "3")
					Debug.Log ("Connected successfully");

				AuthEvent authEvent = new AuthEvent ();
				authEvent.DeviceId = "shahin";
				authEvent.UserId = "bardia";
				authEvent.PrivateKey = "0212526";

				TrySendData (AuthEvent.SerializeToBytes (authEvent));

				int n = _clientSocket.Receive (buffer);
				bytes = new byte[n - 2];
				Buffer.BlockCopy (buffer, 2, bytes, 0, n - 2);
				if (Response.Deserialize (bytes).Type == "6")
					Debug.Log ("Authenticated succesfully");

				state = AbrioState.CONNECTED;
				retryCount = 0;
				//handler.onConnected();
				_clientSocket.BeginReceive(buffer, 0, 2, SocketFlags.None, new AsyncCallback(ReceiveFrameCallback), null);
				Debug.Log("Starting listening");

			} catch (SocketException ex) {
				Debug.Log ("Fail to connect: " + ex.Message + ex.StackTrace);
				reconnect ();
			}
		}

		private void reconnect()
		{
			state = AbrioState.IDLE;
			_clientSocket.Close ();
			_clientSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			connect_server ();
		}
		private int delayPolicy (int retry_count)
		{
			return (int)Math.Pow (2, retry_count);
		}

		public void Send(string title,string body){
			if (state == AbrioState.CONNECTED) {
				EventWrapper newEvent = new EventWrapper ();
				BasicEvent basicEvent = new BasicEvent ();
				basicEvent.Title = title;
				basicEvent.Body = body;
				newEvent.eventType = EventWrapper.EventType.BasicEvent;
				newEvent.BasicEvent = basicEvent;
				TrySendData (EventWrapper.SerializeToBytes (newEvent));
			}
		}

		private bool TrySendData (byte[] data)
		{
			Debug.Log("Send data");
			try {
				int len = data.Length;
				byte[] data_with_header = new byte[len + 2];
				data_with_header [0] = (byte)(len / 0x00FF);
				data_with_header [1] = (byte)(len % 0x00FF);
				data.CopyTo (data_with_header, 2);

				int sent_bytes = _clientSocket.Send (data_with_header);
				if (sent_bytes != data_with_header.Length)
					Debug.Log ("Except to send " + data_with_header.Length.ToString () + " but send " + sent_bytes.ToString ());
				return true;
			} catch (Exception e) {
				Debug.Log ("Error in TrySendData " + e.StackTrace);
				reconnect ();
				return false;
			}
		}

		private void ReceiveFrameCallback (IAsyncResult AR)
		{
			int recieved = _clientSocket.EndReceive (AR);
			Debug.Log ("Frame came");
			if (recieved != 2) {
				Debug.Log ("wrong frame size");
				return;
			}

			int size = ((int)(buffer [0]) << 4) + (int)(buffer [1]);
			_clientSocket.BeginReceive (buffer, 0, size, SocketFlags.None, new AsyncCallback (ReceiveCallback), null);
		}

		private void ReceiveCallback (IAsyncResult AR)
		{
			//Check how much bytes are recieved and call EndRecieve to finalize handshake
			int recieved = _clientSocket.EndReceive (AR);

			if (recieved <= 0)
				return;

			//Copy the recieved data into new buffer , to avoid null bytes
			//now  all Data is in recData
			byte[] recData = new byte[recieved];
			Buffer.BlockCopy (buffer, 0, recData, 0, recieved);

			Debug.Log ("fuuuck");
			handler.onBasicEvent (EventWrapper.Deserialize (recData).BasicEvent);

			//Start receiving again
			_clientSocket.BeginReceive(buffer, 0, 2, SocketFlags.None, new AsyncCallback(ReceiveFrameCallback), null);
		}

	}
}

