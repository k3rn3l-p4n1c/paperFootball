using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using Abrio;
using System.Net;
using System.Collections.Generic;
using System.Threading;


public class SocketHandler: IAbrioHandler
{
    OutterWorldState worldState;
    GameLogic gameLogic;

    public SocketHandler(OutterWorldState ws, GameLogic gl)
    {
        worldState = ws;
        gameLogic = gl;
    }

	void IAbrioHandler.onConnected ()
	{
		throw new NotImplementedException ();
	}

	void IAbrioHandler.onBasicEvent (BasicEvent abrioEvent)
	{
		string[] serverData = abrioEvent.Body.Split(' ');
		string head = serverData[0].Trim();
		//Debug.Log (incommingEvent.Title + " - " + worldState.turn);
		Debug.Log(abrioEvent.Title);
		if (abrioEvent.Title != worldState.turn.ToString())
		{
			switch (head)
			{
			case "Ball":
				worldState.update(
					new Vector3(-float.Parse(serverData[1]), -float.Parse(serverData[2])),
					new Vector3(-float.Parse(serverData[3]), -float.Parse(serverData[4])),
					new Vector3(-float.Parse(serverData[5]), -float.Parse(serverData[6])),
					new Vector3(-float.Parse(serverData[7]), -float.Parse(serverData[8])),
					new Vector3(-float.Parse(serverData[9]), -float.Parse(serverData[10])),
					new Vector3(-float.Parse(serverData[11]), -float.Parse(serverData[12])));
				break;
			case "Turn":
				worldState.turn = Int32.Parse(serverData[1]);
				Debug.Log("TURRRRN: " + worldState.turn.ToString());
				gameLogic.StateMachine.SetTurn(worldState.turn);
				break;
			case "ChTurn":
				gameLogic.StateMachine.YourTurn();
				break;
			default:
				Debug.Log ("invalid msg from server");
				Debug.Log (abrioEvent.Title);
				Debug.Log (abrioEvent.Body);
				break;
			}
		}
	}
}