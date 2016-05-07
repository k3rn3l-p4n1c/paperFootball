using UnityEngine;
using System.Collections;

/*
 * this class contains Player info which we get from  
 * Server By Socket
 */
public class OutterWorldState {
	private bool readyToRead = false;

	public Vector3 ball1Pos, ball2Pos, ball3Pos;
	public Vector3 ball1Vel, ball2Vel, ball3Vel;

	public void Read(){
		readyToRead = false;
	}

	public bool IsReady(){
		return readyToRead;
	}

	public void update(Vector3 ball1Pos,Vector3  ball2Pos,Vector3  ball3Pos,Vector3 ball1Vel,Vector3  ball2Vel,Vector3  ball3Vel){
		this.ball1Pos = ball1Pos;
		this.ball2Pos = ball2Pos;
		this.ball3Pos = ball3Pos;
		this.ball1Vel = ball1Vel;
		this.ball2Vel = ball2Vel;
		this.ball3Vel = ball3Vel;
		readyToRead = true;
	}
}
