using UnityEngine;
using System.Collections;

/*
 * this class contains Player info which we get from  
 * Server By Socket
 */
public class OutterWorldState {

	static private OutterWorldState instance = null;
	private OutterWorldState() {
	}

	static public OutterWorldState i(){
		if(OutterWorldState.instance == null)
			instance = new OutterWorldState();
		return instance;
	}

	private bool readyToRead = false;

	public Vector3 ball1Pos, ball2Pos, ball3Pos;
	public Vector3 ball1Vel, ball2Vel, ball3Vel;
	public int turn;

	public void Read(){
		readyToRead = false;
	}

	public bool IsReady(){
		return readyToRead;
	}

	public void update(Vector3 _ball1Pos,Vector3  _ball1Vel,Vector3 _ball2Pos,Vector3  _ball2Vel,Vector3 _ball3Pos,Vector3  _ball3Vel){
		this.ball1Pos = _ball1Pos;
		this.ball2Pos = _ball2Pos;
		this.ball3Pos = _ball3Pos;
		this.ball1Vel = _ball1Vel;
		this.ball2Vel = _ball2Vel;
		this.ball3Vel = _ball3Vel;
		readyToRead = true;
	}
}
